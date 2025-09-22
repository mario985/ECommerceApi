using System.ComponentModel.DataAnnotations;

public class UpdateCartItemQuantityDto
{
    [Required]
    public string UserId { set; get; }
    [Required]
    public string ProductId { set; get; }
    [Required]
    [Range(0 , 1000)]
    public int quantity { set; get; }
}