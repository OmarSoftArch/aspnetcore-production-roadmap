namespace SalesInventory.Api.DTOs;

public record CategoryResponse(
    int Id,
    string Name,
    string Description,
    bool IsActive);
