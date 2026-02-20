using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("category")]
    [BsonRepresentation(BsonType.String)]
    public ProductCategory Category { get; set; }

    [BsonElement("price")]
    [BsonRepresentation(BsonType.Double)]
    public decimal Price { get; set; }

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;
    [BsonElement("brand")]
    public string Brand { get; set; } = string.Empty;
    [BsonElement("isAvailable")]
    public bool IsAvailable{ get; set; }
}
