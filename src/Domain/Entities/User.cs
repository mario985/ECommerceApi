using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public List<ShippingAddress>? Address { set; get; }
    public ICollection<Order>? Orders { set; get; }
    public Cart? Cart { set; get; }
    public WishList? WishList { set; get; }
    public List<RefreshToken>? RefreshTokens { set; get; } = new();
    public string? GoogleId {set;get;}

}