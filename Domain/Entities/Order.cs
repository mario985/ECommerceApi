using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int Id { set; get; }
    [ForeignKey("User")]
    public string? UserId { set; get; }
    public User User{ set; get; }
    public PaymentMethod paymentMethod { set; get; }
    public string Currency {set ; get ;}
    public OrderStatus Status { set; get; }
    public decimal TotalPrice { set; get; }
    public DateTime CreatedAt { set; get; }
    public ICollection<OrderItem>? OrderItems { set; get; }
    public string? StripePaymentIntentId { get; set; }
    public DateTime? PaymentConfirmedAt { get; set; }
    public DateTime? ExpiresAt {set;get;}

    
    
}