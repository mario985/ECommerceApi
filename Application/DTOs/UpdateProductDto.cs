using System.ComponentModel.DataAnnotations;

public class UpdateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public ProductCategory Category { get; set; }

    [Required]
    [Range(1, 1000000)]
    public decimal Price { get; set; }

    [Url]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Brand { get; set; } = string.Empty;
    [Required]
    [Range(0, 100000)]
    public int Quantity{ set; get; }
}
