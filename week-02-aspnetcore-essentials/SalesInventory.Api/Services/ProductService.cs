using SalesInventory.Api.DTOs;
using SalesInventory.Api.Models;

namespace SalesInventory.Api.Services;

public class ProductService : IProductService
{
    private readonly ICategoryService _categoryService;
    private readonly object _syncRoot = new();
    private int _nextProductId = 4;

    private readonly List<Product> _products =
    [
        new Product
        {
            Id = 1,
            Name = "Inventory Management Subscription",
            Description = "Cloud subscription for tracking products and stock levels",
            Price = 149.99m,
            StockQuantity = 100,
            CategoryId = 1
        },
        new Product
        {
            Id = 2,
            Name = "Barcode Scanner",
            Description = "Handheld scanner for warehouse receiving and shipping",
            Price = 89.50m,
            StockQuantity = 25,
            CategoryId = 2
        },
        new Product
        {
            Id = 3,
            Name = "Standing Desk",
            Description = "Adjustable office desk for internal teams",
            Price = 399.00m,
            StockQuantity = 12,
            CategoryId = 3
        }
    ];

    public ProductService(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IReadOnlyList<ProductResponse> GetAll()
    {
        lock (_syncRoot)
        {
            return _products
                .Select(ToResponse)
                .ToList();
        }
    }

    public ProductResponse? GetById(int id)
    {
        lock (_syncRoot)
        {
            Product? product = _products.FirstOrDefault(product => product.Id == id);

            return product is null
                ? null
                : ToResponse(product);
        }
    }

    public ProductResponse Create(CreateProductRequest request)
    {
        lock (_syncRoot)
        {
            Product product = new()
            {
                Id = _nextProductId++,
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                IsActive = request.IsActive
            };

            _products.Add(product);

            return ToResponse(product);
        }
    }

    public ProductResponse? Update(int id, UpdateProductRequest request)
    {
        lock (_syncRoot)
        {
            Product? product = _products.FirstOrDefault(product => product.Id == id);

            if (product is null)
                return null;

            product.Name = request.Name.Trim();
            product.Description = request.Description.Trim();
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.CategoryId = request.CategoryId;
            product.IsActive = request.IsActive;

            return ToResponse(product);
        }
    }

    public bool Delete(int id)
    {
        lock (_syncRoot)
        {
            Product? product = _products.FirstOrDefault(product => product.Id == id);

            if (product is null)
                return false;

            _products.Remove(product);

            return true;
        }
    }

    public bool Exists(int id)
    {
        lock (_syncRoot)
        {
            return _products.Any(product => product.Id == id);
        }
    }

    private ProductResponse ToResponse(Product product)
    {
        CategoryResponse? category = _categoryService.GetById(product.CategoryId);
        string categoryName = category?.Name ?? "Unknown Category";

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.CategoryId,
            categoryName,
            product.IsActive);
    }
}
