using System.ComponentModel.DataAnnotations;

public class RemoveFromWishListDto
{
    public string? UserId { set; get; } = string.Empty;
    [Required]
    public string ProductId { set; get; }
    public RemoveFromWishListDto(string userid, string productid)
    {
        UserId = userid;
        ProductId = productid;
    }
    
}