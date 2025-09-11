public interface IProductService
{
     Task<ProductDto?> GetProductById(string Id);
    Task<List<ProductDto>> GetProductsAsync(ProdcutFilterDto prodcutFilterDto);
    Task AddProductAsync(CreateProductDto product);
    Task<UpdateResult> UpdateProductAsync(string id, UpdateProductDto product);
    Task<UpdateResult> DeleteProductAsync(string id);
}