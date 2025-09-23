using System.Text.Json.Serialization;
public class WishListItemDto
{
    public int Id { set; get; }
    [JsonIgnore]
    public string ProductId { set; get; }
    public ProductDto Product { set; get; }
}