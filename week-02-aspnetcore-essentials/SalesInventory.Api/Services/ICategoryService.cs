using SalesInventory.Api.DTOs;

namespace SalesInventory.Api.Services;

public interface ICategoryService
{
    IReadOnlyList<CategoryResponse> GetAll();
    CategoryResponse? GetById(int id);
    CategoryResponse Create(CreateCategoryRequest request);
    CategoryResponse? Update(int id, UpdateCategoryRequest request);
    bool Delete(int id);
    bool Exists(int id);
}
