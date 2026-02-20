using System.ComponentModel.DataAnnotations.Schema;

public class Cart
{
    public int Id { set; get; }
    [ForeignKey("User")]
    public string UserId { set; get; } = null!;
    public User User { set; get; } = null!;
    public ICollection<CartItem>? CartItems { set; get; }
    
}