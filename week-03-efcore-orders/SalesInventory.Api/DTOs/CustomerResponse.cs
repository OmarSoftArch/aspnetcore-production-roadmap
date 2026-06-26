namespace SalesInventory.Api.DTOs;

public record CustomerResponse(
    int Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    bool IsActive);
