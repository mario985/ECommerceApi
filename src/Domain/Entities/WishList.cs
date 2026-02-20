using System.ComponentModel.DataAnnotations.Schema;

public class WishList
{
    public int Id { set; get; }
    [ForeignKey("User")]
    public string UserId { set; get; } = null!;
    public User User { set; get; } = null!;
    public ICollection<WishListItem>? WishListItems{ set; get; }
}