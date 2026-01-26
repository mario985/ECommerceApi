using System.Text.Json.Serialization;

public class CartItemDto
{
    public int Id { set; get; }
    public int Quantity { set; get; }
    [JsonIgnore]
    public string ProductId { set; get; }
    public decimal Price{ set; get; }
    
}