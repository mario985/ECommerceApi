using System.ComponentModel.DataAnnotations.Schema;

public class OrderItem
{
    public int Id { set; get; }
    [ForeignKey("Order")]
    public int OrderId { set; get; }
    public Order? Order { set; get; }
    public string ProductId { set; get; } = string.Empty;
    public int Quantity { set; get; }
    public decimal Price{ set; get; }
    
}