using System.ComponentModel.DataAnnotations.Schema;

public class CartItem
{
    public int id { set; get; }
    [ForeignKey("Cart")]
    public int CartId { set; get; }
    public Cart Cart { set; get; } = null!;
    [ForeignKey("Product")]   
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { set; get; }
    
}