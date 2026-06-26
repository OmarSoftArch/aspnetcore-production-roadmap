using System.ComponentModel.DataAnnotations;

namespace SalesInventory.Api.DTOs;

public class UpdateCategoryRequest
{
    [Required]
    [StringLength(80, MinimumLength = 2)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Name must contain at least one non-whitespace character.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(250, MinimumLength = 5)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Description must contain at least one non-whitespace character.")]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
