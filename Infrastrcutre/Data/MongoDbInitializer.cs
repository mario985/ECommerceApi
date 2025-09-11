using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class MongoDbInitializer
{
    private readonly IMongoCollection<Product> _products;

     public MongoDbInitializer(IOptions<MongoDbSetting> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
        var dataBase = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _products = dataBase.GetCollection<Product>(mongoDbSettings.Value.CollectionName);
    }

    public async Task CreateIndexesAsync()
    {
        var indexModels = new List<CreateIndexModel<Product>>
        {
            new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Ascending(p=>p.Name)
            ),
            new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Ascending(p=>p.Price)
            ),
            new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Ascending(p=>p.Category)
            )

        };

        await _products.Indexes.CreateManyAsync(indexModels);
    }
}
