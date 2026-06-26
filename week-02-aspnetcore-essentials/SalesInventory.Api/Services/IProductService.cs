using SalesInventory.Api.DTOs;

namespace SalesInventory.Api.Services;

public interface IProductService
{
    IReadOnlyList<ProductResponse> GetAll();
    ProductResponse? GetById(int id);
    ProductResponse Create(CreateProductRequest request);
    ProductResponse? Update(int id, UpdateProductRequest request);
    bool Delete(int id);
    bool Exists(int id);
}
