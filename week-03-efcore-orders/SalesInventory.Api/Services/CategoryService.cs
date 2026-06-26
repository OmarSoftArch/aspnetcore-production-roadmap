using Microsoft.EntityFrameworkCore;
using SalesInventory.Api.Data;
using SalesInventory.Api.DTOs;
using SalesInventory.Api.Models;

namespace SalesInventory.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly SalesInventoryDbContext _dbContext;

    public CategoryService(SalesInventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Id)
            .Select(category => ToResponse(category))
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .Where(category => category.Id == id)
            .Select(category => ToResponse(category))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        Category category = new()
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            IsActive = request.IsActive
        };

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(category);
    }

    public async Task<CategoryResponse?> UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        Category? category = await _dbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null)
            return null;

        category.Name = request.Name.Trim();
        category.Description = request.Description.Trim();
        category.IsActive = request.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(category);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        Category? category = await _dbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null || !category.IsActive)
            return false;

        category.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AnyAsync(category => category.Id == id && category.IsActive, cancellationToken);
    }

    private static CategoryResponse ToResponse(Category category)
    {
        return new CategoryResponse(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive);
    }
}
