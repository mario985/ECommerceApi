public interface IProductService
{
     Task<ProductDto?> GetProductById(string Id);
    Task<List<ProductDto>> GetProducts(ProdcutFilterDto prodcutFilterDto);
    Task AddProductAsync(CreateProductDto product);
    Task<bool> UpdateProductAsync(string id, UpdateProductDto product);
    Task<bool> DeleteProductAsync(string id);
}