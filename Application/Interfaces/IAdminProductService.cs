public interface IAdminProductService
{
    Task AddProductAsync(CreateProductDto product);
    Task<UpdateResult> UpdateProductAsync(string id, UpdateProductDto product);
    Task<UpdateResult> DeleteProductAsync(string id);
}