using System.Text.Json;
using AutoMapper;
using ZstdSharp.Unsafe;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;


    public CartService(ICartRepository cartRepository, IMapper mapper, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
        _productRepository = productRepository;
    }
    public async Task<CartDto?> AddItemAsync(AddToCartDto addToCartDto)
    {

        var cart = await _cartRepository.AddToCartAsync(addToCartDto.userId, addToCartDto.productId, addToCartDto.Quantity);
        return await MapCartToDtoAsync(cart);

    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        return await _cartRepository.ClearCartAsync(userId);
    }

    public async Task<CartDto?> GetCartAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId)
               ?? await _cartRepository.AddCartAsync(userId);
        return await MapCartToDtoAsync(cart);

    }

    public async Task<CartApiResult> RemoveFromCartAsync(RemoveFromCartDto removeFromCartDto)
    {
        var result = await _cartRepository.DeleteFromCartAsync(removeFromCartDto.UserId, removeFromCartDto.ProductId);
        var cartDto = await MapCartToDtoAsync(result.Cart);
        return new CartApiResult { Cart = cartDto, IsSuccess = result.IsSuccess, Message = result.Message };


    }

    public async Task<CartApiResult> UpdateItemQuantityAsync(UpdateCartItemQuantityDto updateCartItemQuantityDto)
    {
        if (updateCartItemQuantityDto.quantity == 0)
            return await RemoveFromCartAsync(new RemoveFromCartDto { UserId = updateCartItemQuantityDto.UserId, ProductId = updateCartItemQuantityDto.ProductId });

        var result = await _cartRepository.UpdateItemQuantityAsync(updateCartItemQuantityDto.UserId, updateCartItemQuantityDto.ProductId, updateCartItemQuantityDto.quantity);
        var cartDto = await MapCartToDtoAsync(result.Cart);
        return new CartApiResult { Cart = cartDto, IsSuccess = result.IsSuccess, Message = result.Message };
    }
    private async Task<CartDto?> MapCartToDtoAsync(Cart? cart)
    {
        if (cart == null) return null;
        return _mapper.Map<CartDto>(cart);
        
    }
}