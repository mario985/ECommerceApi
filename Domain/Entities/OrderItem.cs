using System.ComponentModel.DataAnnotations.Schema;

public class OrderItem
{
    public int Id { set; get; }
    [ForeignKey("Order")]
    public int OrderId { set; get; }
    public Order? Order { set; get; }
    [ForeignKey("Product")]
    public int ProductId { set; get; }
    public Product? Product { set; get; }
    public int Quantity { set; get; }
    public decimal Price{ set; get; }
    
}