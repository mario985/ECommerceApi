using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;
[Route("Api/[controller]")]
[ApiController]
[Authorize]
public class WishListController : ControllerBase
{
    private readonly IWishListService _wishListService;
    public WishListController(IWishListService wishListService) => _wishListService = wishListService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string userId = GetUserId();
        var wishList = await _wishListService.GetWishListAsync(userId);
        return Ok(wishList);
    }
    [HttpPost]
    public async Task<IActionResult> AddItem(AddToWishListDto addToWishListDto)
    {
        string userId = GetUserId();
        addToWishListDto.UserId = userId;
        var wishList = await _wishListService.AddToWishListAsync(addToWishListDto);
        return Ok(wishList);

    }
    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveItem(string productId )
    {
        string userId = GetUserId();
        RemoveFromWishListDto removeFromWishListDto = new RemoveFromWishListDto(userId , productId);
        var wishList = await _wishListService.RemoveFromWishListAsync(removeFromWishListDto);
        return Ok(wishList);

    }
    [HttpDelete("clear")]
    public async Task<IActionResult> Clear()
    {
        string userId = GetUserId();
        var wishList = await _wishListService.ClearWishListAsync(userId);
        return Ok(wishList);


    }
    private string GetUserId()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userId))
        throw new UnauthorizedAccessException("User not authenticated.");
    return userId;
}
}