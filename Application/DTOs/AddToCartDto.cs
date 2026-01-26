using System.ComponentModel.DataAnnotations;

public class AddToCartDto
{
    
    public string? userId { set; get; } = string.Empty;
    [Required]
    public string productId { set; get; }
    [Required]
    [Range(1 , 1000)]
    public int Quantity { set; get; }
}