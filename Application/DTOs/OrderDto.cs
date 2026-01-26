using System.ComponentModel.DataAnnotations.Schema;

public class OrderDto
{
    public PaymentMethod paymentMethod { set; get; }
    public OrderStatus Status { set; get; }
    public decimal TotalPrice { set; get; }
    public DateTime CreatedAt { set; get; }
    public ICollection<OrderItemDto>? OrderItems { set; get; }
    
    
}