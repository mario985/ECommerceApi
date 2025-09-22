using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    public int id { set; get; }
    [ForeignKey("Cart")]
    public int CartId { set; get; }
    public Cart Cart { set; get; } = null!;
    public string ProductId { get; set; } = string.Empty;
    public Product Product{ set; get; }
    public int Quantity { set; get; }
    
}