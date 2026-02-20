using System.Text.Json;
using AutoMapper;
using MediatR;
public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IRedisCacheService _redisCacheService;
    public ProductService(IProductRepository productRepository, IMapper mapper, IRedisCacheService redisCacheService)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _redisCacheService = redisCacheService;

    }
public async Task<List<ProductDto>> GetProductsAsync(ProdcutFilterDto prodcutFilterDto)
{
    string cachedKey = $"search:{prodcutFilterDto.Name}:{prodcutFilterDto.Category}:{prodcutFilterDto.MinPrice}:{prodcutFilterDto.MaxPrice}:{prodcutFilterDto.Page}:{prodcutFilterDto.Limit}:{prodcutFilterDto.SortBy}";

    var result = await _redisCacheService.GetAsync(cachedKey);
    if (!string.IsNullOrEmpty(result))
    {
        Console.WriteLine("Cache Hit");
        try
        {
            var productsFromCache = JsonSerializer.Deserialize<List<Product>>(result);
            return _mapper.Map<List<ProductDto>>(productsFromCache);
        }
        catch
        {
            Console.WriteLine("Cache corrupted, fallback to DB");
        }
    }

    Console.WriteLine("Cache Miss");
    var productsFromDb = await _productRepository.GetProductsAsync(
        prodcutFilterDto.Name,
        prodcutFilterDto.Category,
        prodcutFilterDto.MaxPrice,
        prodcutFilterDto.MinPrice,
        prodcutFilterDto.SortBy,
        prodcutFilterDto.Brand,
        prodcutFilterDto.SortDec,
        prodcutFilterDto.Page,
        prodcutFilterDto.Limit
    );

    string serialized = JsonSerializer.Serialize(productsFromDb);
    await _redisCacheService.SetAsync(cachedKey, serialized, TimeSpan.FromMinutes(10));

    return _mapper.Map<List<ProductDto>>(productsFromDb);
}


    public async Task<ProductDto?> GetProductById(string Id)
    {
        string cachedKey = $"Product Id :{Id}";
        string result = await _redisCacheService.GetAsync(cachedKey);
        if (!string.IsNullOrEmpty(result))
        {
            Console.WriteLine("CacheHit");
            try
            {
                var productFromCache = JsonSerializer.Deserialize<Product>(result);
                return _mapper.Map<ProductDto>(productFromCache);
            }
            catch
            {
                Console.WriteLine("Cache Error fallback to DB");
            }
        }
        Console.WriteLine("Cache miss");
        var productFromDb = await _productRepository.GetById(Id);
        string serialized = JsonSerializer.Serialize(productFromDb);
        if (productFromDb == null)
        {
            await _redisCacheService.SetAsync(cachedKey, serialized, TimeSpan.FromMinutes(2));
            return null;
        }
        await _redisCacheService.SetAsync(cachedKey, serialized, TimeSpan.FromHours(1));
        return _mapper.Map<ProductDto>(productFromDb);

    }
   
}