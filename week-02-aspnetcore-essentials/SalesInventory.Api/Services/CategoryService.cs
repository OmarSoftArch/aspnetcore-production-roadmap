using SalesInventory.Api.DTOs;
using SalesInventory.Api.Models;

namespace SalesInventory.Api.Services;

public class CategoryService : ICategoryService
{
    // Temporary in-memory data for week 2.
    // EF Core and a real database are part of the next week in the roadmap.
    private readonly object _syncRoot = new();
    private int _nextCategoryId = 4;

    private readonly List<Category> _categories =
    [
        new Category
        {
            Id = 1,
            Name = "Software Subscriptions",
            Description = "Monthly and yearly SaaS products for business customers"
        },
        new Category
        {
            Id = 2,
            Name = "Warehouse Devices",
            Description = "Devices used in inventory and warehouse operations"
        },
        new Category
        {
            Id = 3,
            Name = "Office Equipment",
            Description = "Equipment used by internal business teams"
        }
    ];

    public IReadOnlyList<CategoryResponse> GetAll()
    {
        lock (_syncRoot)
        {
            return _categories
                .Select(ToResponse)
                .ToList();
        }
    }

    public CategoryResponse? GetById(int id)
    {
        lock (_syncRoot)
        {
            Category? category = _categories.FirstOrDefault(category => category.Id == id);

            return category is null
                ? null
                : ToResponse(category);
        }
    }

    public CategoryResponse Create(CreateCategoryRequest request)
    {
        lock (_syncRoot)
        {
            Category category = new()
            {
                Id = _nextCategoryId++,
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                IsActive = request.IsActive
            };

            _categories.Add(category);

            return ToResponse(category);
        }
    }

    public CategoryResponse? Update(int id, UpdateCategoryRequest request)
    {
        lock (_syncRoot)
        {
            Category? category = _categories.FirstOrDefault(category => category.Id == id);

            if (category is null)
                return null;

            category.Name = request.Name.Trim();
            category.Description = request.Description.Trim();
            category.IsActive = request.IsActive;

            return ToResponse(category);
        }
    }

    public bool Delete(int id)
    {
        lock (_syncRoot)
        {
            Category? category = _categories.FirstOrDefault(category => category.Id == id);

            if (category is null)
                return false;

            _categories.Remove(category);

            return true;
        }
    }

    public bool Exists(int id)
    {
        lock (_syncRoot)
        {
            return _categories.Any(category => category.Id == id);
        }
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
