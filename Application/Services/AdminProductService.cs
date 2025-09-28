using AutoMapper;
using MediatR;

public class AdminProductService : IAdminProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IRedisCacheService _redisCacheService;
    private readonly IMediator _mediator;
    public AdminProductService(IProductRepository productRepository, IMapper mapper, IRedisCacheService redisCacheService, IMediator mediator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _redisCacheService = redisCacheService;
        _mediator = mediator;
    }
    public async Task AddProductAsync(CreateProductDto productDto)
    {

        var product = _mapper.Map<Product>(productDto);
        product.IsAvailable = (productDto.Quantity > 0) ? true : false;
        await _productRepository.CreateAsync(product);
        await _mediator.Publish(new ProductCreated(product.Id, productDto.Quantity));
        return;
    }
    public async Task<UpdateResult> UpdateProductAsync(string id, UpdateProductDto productDto)
    {

        var product = _mapper.Map<Product>(productDto);
        product.IsAvailable = (productDto.Quantity > 0) ? true : false;
        var result = await _productRepository.UpdateAsync(id, product);
        if (result.Success == true)
        {
            await _redisCacheService.RemoveAsync($"Product Id :{id}");
        }
        await _mediator.Publish(new ProductUpdatedCommand(product.Id, productDto.Quantity));
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

    public async Task ChangeProductAvailabilityAsync(string productId)
    {
        var product = await _productRepository.GetById(productId);
        if (product != null)
        {
            product.IsAvailable = false;
            await _productRepository.UpdateAsync(productId, product);
        }
        await _redisCacheService.RemoveAsync($"Product Id :{productId}");


    }
}