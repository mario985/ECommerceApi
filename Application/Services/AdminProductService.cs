using AutoMapper;

public class AdminProductService : IAdminProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IRedisCacheService _redisCacheService;
    public AdminProductService(IProductRepository productRepository, IMapper mapper, IRedisCacheService redisCacheService)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _redisCacheService = redisCacheService;
    }
    public async Task AddProductAsync(CreateProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.CreateAsync(product);
        return;
    }
    public async Task<UpdateResult> UpdateProductAsync(string id, UpdateProductDto productDto)
    {

        var product = _mapper.Map<Product>(productDto);
        var result = await _productRepository.UpdateAsync(id, product);
        if (result.Success == true)
        {
            await _redisCacheService.RemoveAsync($"Product Id :{id}");
        }
        return result;
        
    }

    public async Task<UpdateResult> DeleteProductAsync(string id)
    {
        var result = await _productRepository.DeleteAsync(id);
        if (result.Success == true)
        {
            await _redisCacheService.RemoveAsync($"Product Id :{id}");
        }
        return result;
        
       
    }
}