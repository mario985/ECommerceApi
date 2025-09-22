public interface ICartService
{
    Task<CartDto?> AddItemAsync(AddToCartDto addToCartDto);
    Task<CartApiResult> RemoveFromCartAsync(RemoveFromCartDto removeFromCartDto);
    Task<bool> ClearCartAsync(string userId);
    Task<CartDto?> GetCartAsync(string userId);
    Task<CartApiResult> UpdateItemQuantityAsync(UpdateCartItemQuantityDto updateCartItemQuantityDto);
}