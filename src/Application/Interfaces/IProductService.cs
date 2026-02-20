public interface IProductService
{
     Task<ProductDto?> GetProductById(string Id);
    Task<List<ProductDto>> GetProductsAsync(ProdcutFilterDto prodcutFilterDto);
}