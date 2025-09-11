public interface IProductRepository
{
    Task<Product?> GetById(string Id);
    Task CreateAsync(Product product);
    Task<bool> UpdateAsync(string id, Product product);
    Task<bool> DeleteAsync(string id);
    Task<List<Product>> GetProductsAsync(
   ProductCategory? category,
   decimal? minPrice ,
   decimal? maxPrice,
   string? sortBy,
   string brand,
   bool sortDesc,
   int page,
   int limit
   );
}