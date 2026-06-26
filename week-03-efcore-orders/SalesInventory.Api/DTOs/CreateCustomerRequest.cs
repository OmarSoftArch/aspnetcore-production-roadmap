using System.ComponentModel.DataAnnotations;

namespace SalesInventory.Api.DTOs;

public class CreateCustomerRequest
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    [RegularExpression(@".*\S.*", ErrorMessage = "FullName must contain at least one non-whitespace character.")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(180)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;
}
