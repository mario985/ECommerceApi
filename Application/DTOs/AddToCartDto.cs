using System.ComponentModel.DataAnnotations;

public class AddToCartDto
{
    [Required]
    public string userId { set; get; }
    [Required]
    public string productId { set; get; }
    [Required]
    [Range(1 , 1000)]
    public int Quantity { set; get; }
}