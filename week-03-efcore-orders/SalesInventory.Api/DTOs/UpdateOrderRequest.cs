using System.ComponentModel.DataAnnotations;
using SalesInventory.Api.Models;

namespace SalesInventory.Api.DTOs;

public class UpdateOrderRequest
{
    public OrderStatus Status { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
