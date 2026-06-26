using System.ComponentModel.DataAnnotations;

namespace SalesInventory.Api.DTOs;

public class UpdateProductRequest
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Name must contain at least one non-whitespace character.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 5)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Description must contain at least one non-whitespace character.")]
    public string Description { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "1000000")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public bool IsActive { get; set; } = true;
}
