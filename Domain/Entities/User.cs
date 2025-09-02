using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string Address { set; get; }
    public ICollection<Order>? Orders { set; get; }
    public Cart? Cart { set; get; }
    public WishList? WishList { set; get; }
    

}