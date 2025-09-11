

using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
        public async Task<List<ProductDto>> GetProducts(ProdcutFilterDto prodcutFilterDto)
    {
        var products = await _productRepository.GetProductsAsync
        (prodcutFilterDto.Category,
         prodcutFilterDto.MaxPrice,
          prodcutFilterDto.MinPrice,
          prodcutFilterDto.SortBy,
          prodcutFilterDto.Brand,
          prodcutFilterDto.SortDec,
          prodcutFilterDto.Page,
          prodcutFilterDto.Limit
        );
        return _mapper.Map <List<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductById(string Id)
    {
        var product = await _productRepository.GetById(Id);
        if (product == null) return null;
        return _mapper.Map<ProductDto>(product);

    }
    //admin crud operations
        public async Task AddProductAsync(CreateProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.CreateAsync(product);
        return;
    }
     public async Task<bool> UpdateProductAsync(string id, UpdateProductDto productDto)
    {
        
        var product = _mapper.Map<Product>(productDto);
        return await _productRepository.UpdateAsync(id, product);
        
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        return await _productRepository.DeleteAsync(id);
       
    }
}