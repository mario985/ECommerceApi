using System.ComponentModel.DataAnnotations;

public class CreateShippingAddressDto
{
    [Required]
    public string RecipientName { get; set; } = string.Empty;

    [Required]
    public string Line1 { get; set; } = string.Empty;

    public string? Line2 { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public bool IsDefault { get; set; }
}
