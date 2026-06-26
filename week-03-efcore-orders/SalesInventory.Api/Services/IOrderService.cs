using SalesInventory.Api.DTOs;

namespace SalesInventory.Api.Services;

public interface IOrderService
{
    Task<IReadOnlyList<OrderResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<OrderResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ServiceResult<OrderResponse>> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<ServiceResult<OrderResponse>> UpdateAsync(int id, UpdateOrderRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
