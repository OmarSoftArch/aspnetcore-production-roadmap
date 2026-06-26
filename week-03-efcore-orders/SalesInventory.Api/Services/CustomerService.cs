using Microsoft.EntityFrameworkCore;
using SalesInventory.Api.Data;
using SalesInventory.Api.DTOs;
using SalesInventory.Api.Models;

namespace SalesInventory.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly SalesInventoryDbContext _dbContext;

    public CustomerService(SalesInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CustomerResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.Id)
            .Select(customer => ToResponse(customer))
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(customer => customer.Id == id)
            .Select(customer => ToResponse(customer))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        Customer customer = new()
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim(),
            IsActive = request.IsActive
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(customer);
    }

    public async Task<CustomerResponse?> UpdateAsync(int id, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        Customer? customer = await _dbContext.Customers
            .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);

        if (customer is null)
            return null;

        customer.FullName = request.FullName.Trim();
        customer.Email = request.Email.Trim().ToLowerInvariant();
        customer.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim();
        customer.IsActive = request.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(customer);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        Customer? customer = await _dbContext.Customers
            .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);

        if (customer is null || !customer.IsActive)
            return false;

        customer.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Customers
            .AnyAsync(customer => customer.Id == id && customer.IsActive, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludingCustomerId, CancellationToken cancellationToken)
    {
        string normalizedEmail = email.Trim().ToLowerInvariant();

        return await _dbContext.Customers
            .AnyAsync(customer =>
                customer.Email == normalizedEmail &&
                (!excludingCustomerId.HasValue || customer.Id != excludingCustomerId.Value),
                cancellationToken);
    }

    private static CustomerResponse ToResponse(Customer customer)
    {
        return new CustomerResponse(
            customer.Id,
            customer.FullName,
            customer.Email,
            customer.PhoneNumber,
            customer.IsActive);
    }
}
