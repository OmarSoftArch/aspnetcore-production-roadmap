namespace SalesInventory.Api.DTOs;

// Response DTO: controls exactly what the API returns to clients.
// This keeps the external API contract separate from the internal model.
public record CategoryResponse(
    int Id,
    string Name,
    string Description,
    bool IsActive);
