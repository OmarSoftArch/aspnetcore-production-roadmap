using System.ComponentModel.DataAnnotations;

namespace SalesInventory.Api.DTOs;

public class CreateOrderItemRequest
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }
}
