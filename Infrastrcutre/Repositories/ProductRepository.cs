using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
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
    string?name,
    ProductCategory? category,
    decimal? maxPrice,
    decimal? minPrice,
    SortField? sortBy,
    string? brand,
    bool sortDesc,
    int page,
    int limit)
    {
        var filterBuilder = Builders<Product>.Filter;
        var filters = new List<FilterDefinition<Product>>();
        if(!string.IsNullOrEmpty(name))
            filters.Add(filterBuilder.Eq(p => p.Name, name));
        
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

        if (sortBy.HasValue)
        {
            var sortValue = sortBy.ToString();
            sort = sortDesc
                ? sortBuilder.Descending(sortValue)
                : sortBuilder.Ascending(sortValue);
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
    public async Task<UpdateResult> DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return new UpdateResult { Success = false, Message = $"the provided id {id} not a vlalid id" };
        }
        try
        {
            var result = await _products.DeleteOneAsync(p => p.Id == id);
            if (!result.IsAcknowledged)
            {
                return new UpdateResult { Success = false, Message = "Delete method not aknowledged" };

            }
            if (result.DeletedCount == 0)
            {
                return new UpdateResult { Success = false, Message = $"the proudct {id} not found" };

            }
            else return new UpdateResult { Success = true };
        }
        catch (MongoException ex)
        {
            return new UpdateResult { Success = false, Message = "Database error: " + ex.Message };
        }
        
    }
    public async Task<UpdateResult> UpdateAsync(string id, Product product)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return new UpdateResult { Success = false, Message = $"the provided id {id} not a vlalid id" };
        }
        product.Id = id;
         try
        {
            var result = await _products.ReplaceOneAsync(p => p.Id == id, product);
            if (!result.IsAcknowledged)
            {
                return new UpdateResult { Success = false, Message = "Update method not aknowledged" };

            }
            if (result.ModifiedCount == 0)
            {
                return new UpdateResult { Success = true, Message = $"No changes were made" };

            }
            else return new UpdateResult { Success = true };
        }
        catch (MongoException ex)
        {
            return new UpdateResult { Success = false, Message = "Database error: " + ex.Message };
        }

        
    }


}