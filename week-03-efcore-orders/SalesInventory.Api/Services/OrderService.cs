using Microsoft.EntityFrameworkCore;
using SalesInventory.Api.Data;
using SalesInventory.Api.DTOs;
using SalesInventory.Api.Models;

namespace SalesInventory.Api.Services;

public class OrderService : IOrderService
{
    private readonly SalesInventoryDbContext _dbContext;

    public OrderService(SalesInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<OrderResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        List<Order> orders = await _dbContext.Orders
            .AsNoTracking()
            .Include(order => order.Customer)
            .Include(order => order.Items)
                .ThenInclude(item => item.Product)
            .OrderByDescending(order => order.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return orders.Select(ToResponse).ToList();
    }

    public async Task<OrderResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        Order? order = await GetOrderGraph()
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

        return order is null ? null : ToResponse(order);
    }

    public async Task<ServiceResult<OrderResponse>> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        Customer? customer = await _dbContext.Customers
            .FirstOrDefaultAsync(customer => customer.Id == request.CustomerId && customer.IsActive, cancellationToken);

        if (customer is null)
            return ServiceResult<OrderResponse>.Failure("CustomerId does not exist or is inactive.");

        List<IGrouping<int, CreateOrderItemRequest>> requestedItems = request.Items
            .GroupBy(item => item.ProductId)
            .ToList();

        List<int> productIds = requestedItems
            .Select(group => group.Key)
            .ToList();

        List<Product> products = await _dbContext.Products
            .Where(product => productIds.Contains(product.Id))
            .ToListAsync(cancellationToken);

        Order order = new()
        {
            OrderNumber = CreateOrderNumber(),
            CustomerId = request.CustomerId,
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
            Status = OrderStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };

        foreach (IGrouping<int, CreateOrderItemRequest> requestedItem in requestedItems)
        {
            Product? product = products.FirstOrDefault(product => product.Id == requestedItem.Key);

            if (product is null || !product.IsActive)
                return ServiceResult<OrderResponse>.Failure($"ProductId {requestedItem.Key} does not exist or is inactive.");

            int quantity = requestedItem.Sum(item => item.Quantity);

            if (product.StockQuantity < quantity)
                return ServiceResult<OrderResponse>.Failure($"ProductId {product.Id} does not have enough stock.");

            product.StockQuantity -= quantity;

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = quantity,
                UnitPrice = product.Price
            });
        }

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        OrderResponse? response = await GetByIdAsync(order.Id, cancellationToken);

        return ServiceResult<OrderResponse>.Success(response!);
    }

    public async Task<ServiceResult<OrderResponse>> UpdateAsync(int id, UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        Order? order = await GetOrderGraph()
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

        if (order is null)
            return ServiceResult<OrderResponse>.Failure("Order was not found.");

        if (order.Status == OrderStatus.Cancelled && request.Status != OrderStatus.Cancelled)
            return ServiceResult<OrderResponse>.Failure("Cancelled orders cannot be reopened.");

        if (order.Status != OrderStatus.Cancelled && request.Status == OrderStatus.Cancelled)
        {
            RestoreStock(order);
        }

        order.Status = request.Status;
        order.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<OrderResponse>.Success(ToResponse(order));
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        Order? order = await GetOrderGraph()
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

        if (order is null)
            return false;

        if (order.Status != OrderStatus.Cancelled)
        {
            RestoreStock(order);
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private IQueryable<Order> GetOrderGraph()
    {
        return _dbContext.Orders
            .Include(order => order.Customer)
            .Include(order => order.Items)
                .ThenInclude(item => item.Product);
    }

    private static void RestoreStock(Order order)
    {
        foreach (OrderItem item in order.Items)
        {
            item.Product.StockQuantity += item.Quantity;
        }
    }

    private static OrderResponse ToResponse(Order order)
    {
        List<OrderItemResponse> items = order.Items
            .OrderBy(item => item.Id)
            .Select(item => new OrderItemResponse(
                item.Id,
                item.ProductId,
                item.Product.Name,
                item.Quantity,
                item.UnitPrice,
                item.Quantity * item.UnitPrice))
            .ToList();

        return new OrderResponse(
            order.Id,
            order.OrderNumber,
            order.CustomerId,
            order.Customer.FullName,
            order.Status,
            order.CreatedAtUtc,
            order.Notes,
            items,
            items.Sum(item => item.LineTotal));
    }

    private static string CreateOrderNumber()
    {
        string suffix = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();

        return $"SO-{DateTime.UtcNow:yyyyMMddHHmmss}-{suffix}";
    }
}
