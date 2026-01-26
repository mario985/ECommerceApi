using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;
[Route("Api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }
    [HttpPost("Add")]
    [Authorize]
    public async Task<IActionResult> AddToCart(AddToCartDto addToCartDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        addToCartDto.userId = userId;
        var cart = await _cartService.AddItemAsync(addToCartDto);
        if (cart == null)
        {
            return BadRequest(new List<string> { "Error adding item" });
        }
        return Ok(cart);
    }
    [HttpDelete("remove/{userId}/{productId}")]
    [Authorize]
    public async Task<IActionResult> RemoveFromCart(string userId , string productId)
    {
        productId = productId.Trim();
        var result = await _cartService.RemoveFromCartAsync(new RemoveFromCartDto{UserId=userId , ProductId = productId});
        if (!result.IsSuccess && result.Cart != null)
        {
            return Ok(new ApiResponseDto<CartDto>
            (
                true,
                "item couldnt be removed",
                result.Cart,
                new List<string> { result.Message }

            )
            );
        }
        else if (!result.IsSuccess)
            return BadRequest(result.Message);
        else return Ok(result.Cart);

    }
    [HttpPost("Clear")]
    [Authorize]
    public async Task<IActionResult> ClearCart()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _cartService.ClearCartAsync(userId);
        return (result) ? Ok() : BadRequest(new List<string> { "Error couldnt clear cart" });
    }
    [HttpPost("updateQuantity")]
    [Authorize]
    public async Task<IActionResult> UpdateItemQuantity(UpdateCartItemQuantityDto updateCartItemQuantityDto)
    {
        var result = await _cartService.UpdateItemQuantityAsync(updateCartItemQuantityDto);
        if (!result.IsSuccess && result.Cart != null)
        {
            return Ok(new ApiResponseDto<CartDto>
            (
                true,
                "quantity couldn't be changed",
                result.Cart,
                new List<string> { result.Message }

            )
            );
        }
        else if (!result.IsSuccess)
            return BadRequest(result.Message);
        else return Ok(result.Cart);

    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null)
            return NotFound();
        return Ok(cart);

    }
    
}