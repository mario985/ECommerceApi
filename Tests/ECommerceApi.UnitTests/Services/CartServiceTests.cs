using AutoMapper;
using Moq;

public class CartServiceTests
{
    private readonly Mock<ICartRepository>_cartRepositoryMock;
    private readonly Mock<IMapper>_mapperMock;
    private readonly Mock<IProductRepository>_productRepositoryMock;
    private readonly ICartService _cartService;
    public CartServiceTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _mapperMock = new Mock<IMapper>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _cartService = new CartService(_cartRepositoryMock.Object , _mapperMock.Object , _productRepositoryMock.Object);
    }
    [Fact]
    public async Task AddItemAsync_ShouldReturnMappedCart()
    {
        // Arrange
        var cart = new Cart { Id = 1 };
        var cartDto = new CartDto { Id = 1 };

        var addDto = new AddToCartDto
        {
            userId = "user1",
            productId = "5",
            Quantity = 2
        };

        _cartRepositoryMock
            .Setup(x => x.AddToCartAsync(addDto.userId, addDto.productId, addDto.Quantity))
            .ReturnsAsync(cart);

        _mapperMock
            .Setup(x => x.Map<CartDto>(cart))
            .Returns(cartDto);

        // Act
        var result = await _cartService.AddItemAsync(addDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cartDto.Id, result.Id);

        _cartRepositoryMock.Verify(x =>
            x.AddToCartAsync(addDto.userId, addDto.productId, addDto.Quantity),
            Times.Once);
    }
    [Fact]
    public async Task GetCartAsync_ShouldCreateCart_WhenCartDoesNotExist()
    {
        // Arrange
        var userId = "user1";
        var cart = new Cart { Id = 1 };
        var cartDto = new CartDto { Id = 1 };

        _cartRepositoryMock
            .Setup(x => x.GetCartByUserIdAsync(userId))
            .ReturnsAsync((Cart?)null);

        _cartRepositoryMock
            .Setup(x => x.AddCartAsync(userId))
            .ReturnsAsync(cart);

        _mapperMock
            .Setup(x => x.Map<CartDto>(cart))
            .Returns(cartDto);

        // Act
        var result = await _cartService.GetCartAsync(userId);

        // Assert
        Assert.NotNull(result);
        _cartRepositoryMock.Verify(x => x.AddCartAsync(userId), Times.Once);
    }
    [Fact]
    public async Task ClearCartAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
    {
        _cartRepositoryMock
            .Setup(x => x.ClearCartAsync("user1"))
            .ReturnsAsync(true);

        var result = await _cartService.ClearCartAsync("user1");

        Assert.True(result);
    }
    [Fact]
    public async Task RemoveFromCartAsync_ShouldReturnResultFromRepository()
    {
        var cart = new Cart();
        var cartDto = new CartDto();

        var repoResult = new UpdateCartResult
        {
            Cart = cart,
            IsSuccess = true,
            Message = "Removed"
        };

        _cartRepositoryMock
            .Setup(x => x.DeleteFromCartAsync("user1", "1"))
            .ReturnsAsync(repoResult);

        _mapperMock
            .Setup(x => x.Map<CartDto>(cart))
            .Returns(cartDto);

        var dto = new RemoveFromCartDto
        {
            UserId = "user1",
            ProductId = "1"
        };

        var result = await _cartService.RemoveFromCartAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Removed", result.Message);
    }

}