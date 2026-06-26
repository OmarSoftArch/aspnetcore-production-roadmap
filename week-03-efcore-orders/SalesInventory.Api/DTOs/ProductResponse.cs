namespace SalesInventory.Api.DTOs;

public record ProductResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    int CategoryId,
    string CategoryName,
    bool IsActive);
