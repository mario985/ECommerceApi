using System.ComponentModel.DataAnnotations.Schema;

public class ShippingAddress
{
    public int Id { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = string.Empty;

    public User? User { get; set; }

    public string RecipientName { get; set; } = string.Empty;

    public string Line1 { get; set; } = string.Empty;

    public string? Line2 { get; set; }

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public bool IsDefault { get; set; }
}
