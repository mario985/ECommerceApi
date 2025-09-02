using System.ComponentModel.DataAnnotations.Schema;

public class WishListItem
{
    public int Id { set; get; }
    [ForeignKey("WishList")]
    public int WishListId { set; get; }
    public WishList WishList { set; get; } = null!;
    [ForeignKey("Product")]
    public int ProductId { set; get; }
    public Product Product { set; get; } = null!;
}