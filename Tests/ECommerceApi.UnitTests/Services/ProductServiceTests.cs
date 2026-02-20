using Moq;
using FluentAssertions;
using Xunit;
using AutoMapper;
using System.Threading.Tasks;
public class ProductServiceTests
{
    
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IRedisCacheService> _redisCacheServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IProductService _productService;
    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _redisCacheServiceMock = new Mock<IRedisCacheService>();
        _mapperMock = new Mock<IMapper>();
        _productService = new ProductService(
            _productRepositoryMock.Object,
            _mapperMock.Object,
            _redisCacheServiceMock.Object
            );
    }
    [Fact]
    public async Task GetProductAsync_should_return_from_Cache_when_cacheExists()
    {
        //arrange
        var filter = new ProdcutFilterDto
        {
            Name = "test",
            Limit = 10,
            Page = 1,
            Brand = "sam",
        };
        var products = new List<Product>
        {
            new Product {Name = "test" , Id = "1"}
        };
        var serialzed = System.Text.Json.JsonSerializer.Serialize(products);

        _redisCacheServiceMock.Setup(x =>x.GetAsync(It.IsAny<string>()))
        .ReturnsAsync(serialzed);
        _mapperMock.Setup(x =>x.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
        .Returns(new List<ProductDto>
        {
            new ProductDto {Name = "test" , Id = "1"}
        });
        //act
        var result = await _productService.GetProductsAsync(filter);
        //assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
       _productRepositoryMock.Verify(
        x => x.GetProductsAsync(
            It.IsAny<string>(),
            It.IsAny<ProductCategory>(),
            It.IsAny<decimal?>(),
            It.IsAny<decimal?>(),
            It.IsAny<SortField>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>()),
        Times.Never);

    }
    [Fact]
    public async Task GetProductAsync_should_return_from_Repository_when_CacheDoesntExist()
    {
               //arrange
        var filter = new ProdcutFilterDto
        {
            Name = "test",
            Limit = 10,
            Page = 1,
            Brand = "sam",
        };
        var products = new List<Product>
        {
            new Product {Name = "test" , Id = "1"}
        };
        _productRepositoryMock.Setup(x =>x.GetProductsAsync(
             It.IsAny<string>(),
            It.IsAny<ProductCategory>(),
            It.IsAny<decimal?>(),
            It.IsAny<decimal?>(),
            It.IsAny<SortField>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>())
        )
        .ReturnsAsync(products);
     _redisCacheServiceMock.Setup(x =>x.GetAsync(It.IsAny<string>()))
        .ReturnsAsync((string?)null);
        _mapperMock.Setup(x =>x.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
        .Returns(new List<ProductDto>
        {
            new ProductDto {Name = "test" , Id = "1"}
        });
        //act
        var result = await _productService.GetProductsAsync(filter);
        //assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        _productRepositoryMock.Verify(
        x => x.GetProductsAsync(
            It.IsAny<string>(),
            It.IsAny<ProductCategory?>(),
            It.IsAny<decimal?>(),
            It.IsAny<decimal?>(),
            It.IsAny<SortField?>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>()),
        Times.Once);
        _redisCacheServiceMock.Verify(x=>x.SetAsync(
            It.IsAny<string>() ,
             It.IsAny<string>(),
             It.IsAny<TimeSpan>()
             ),Times.Once);

        
    }

}