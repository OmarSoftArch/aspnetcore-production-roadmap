using SalesInventory.Api.DTOs;

namespace SalesInventory.Api.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken);
    Task<CustomerResponse?> UpdateAsync(int id, UpdateCustomerRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, int? excludingCustomerId, CancellationToken cancellationToken);
}
