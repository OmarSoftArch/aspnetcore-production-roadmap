using SalesInventory.Api.Models;

namespace SalesInventory.Api.DTOs;

public record OrderResponse(
    int Id,
    string OrderNumber,
    int CustomerId,
    string CustomerName,
    OrderStatus Status,
    DateTime CreatedAtUtc,
    string? Notes,
    IReadOnlyList<OrderItemResponse> Items,
    decimal TotalAmount);
