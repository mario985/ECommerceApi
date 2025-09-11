using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
public class ProductRepository :IProductRepository
{
    private readonly IMongoCollection<Product> _products;
    public ProductRepository(IOptions<MongoDbSetting> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
        var dataBase = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _products = dataBase.GetCollection<Product>(mongoDbSettings.Value.CollectionName);
    }
    public async Task<List<Product>> GetProductsAsync(
    ProductCategory? category,
    decimal? minPrice,
    decimal? maxPrice,
    string? sortBy,
    string? brand,
    bool sortDesc,
    int page,
    int limit)
    {
        var filterBuilder = Builders<Product>.Filter;
        var filters = new List<FilterDefinition<Product>>();

        if (category.HasValue)
            filters.Add(filterBuilder.Eq(p => p.Category, category));

        if (minPrice.HasValue)
            filters.Add(filterBuilder.Gte(p => p.Price, minPrice.Value));

        if (maxPrice.HasValue)
            filters.Add(filterBuilder.Lte(p => p.Price, maxPrice.Value));

        if (!string.IsNullOrEmpty(brand))
            filters.Add(filterBuilder.Eq(p => p.Brand, brand));

        var filter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

        var sortBuilder = Builders<Product>.Sort;
        SortDefinition<Product>? sort = null;

        if (!string.IsNullOrEmpty(sortBy))
        {
            sort = sortDesc
                ? sortBuilder.Descending(sortBy)
                : sortBuilder.Ascending(sortBy);
        }

        // Pagination
        var skip = (page - 1) * limit;

        var query = _products.Find(filter)
                             .Skip(skip)
                             .Limit(limit);

        if (sort != null)
            query = query.Sort(sort);

        return await query.ToListAsync();
    }
    public async Task<Product?> GetById(string Id)
    {
        return await _products.Find(p => p.Id == Id).FirstOrDefaultAsync();
    }
    public async Task CreateAsync(Product product)
    {
        await _products.InsertOneAsync(product);
    }
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _products.DeleteOneAsync(p => p.Id == id);
        return result.IsAcknowledged && result.DeletedCount  > 0;
        
    }
    public async Task<bool> UpdateAsync(string id, Product product)
    {
        var result = await _products.ReplaceOneAsync(p => p.Id == id, product);
        return result.IsAcknowledged && result.ModifiedCount > 0;

        
    }


}