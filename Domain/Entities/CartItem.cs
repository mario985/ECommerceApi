using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    public int id { set; get; }
    [ForeignKey("Cart")]
    public int CartId { set; get; }
    public string ProductId { get; set; } = string.Empty;
    public int Quantity { set; get; }
    public decimal Price{ set; get; }
    
}