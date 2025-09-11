using System.ComponentModel.DataAnnotations.Schema;

public class WishListItem
{
    public int Id { set; get; }
    [ForeignKey("WishList")]
    public int WishListId { set; get; }
    public WishList WishList { set; get; } = null!;
    public string ProductId { set; get; } = string.Empty;
}