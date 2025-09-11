public interface IProductRepository
{
    Task<Product?> GetById(string Id);
    Task CreateAsync(Product product);
    Task<UpdateResult> UpdateAsync(string id, Product product);
    Task<UpdateResult> DeleteAsync(string id);
    Task<List<Product>> GetProductsAsync(
    string?name,
   ProductCategory? category,
   decimal? minPrice ,
   decimal? maxPrice,
   SortField? sortBy,
   string brand,
   bool sortDesc,
   int page,
   int limit
   );
}