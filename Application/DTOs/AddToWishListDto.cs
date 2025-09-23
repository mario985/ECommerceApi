using System.ComponentModel.DataAnnotations;

public class AddToWishListDto
{
    public string? UserId { set; get; } = string.Empty;
    [Required]
    public string ProductId { set; get; }
    
    
}