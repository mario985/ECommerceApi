public interface ICartRepository
{
    Task<Cart?> AddToCartAsync(string userId, string productId, int quantity);
    Task<UpdateCartResult> DeleteFromCartAsync(string userId, string productId);
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<bool> ClearCartAsync(string userId);
    Task<UpdateCartResult> UpdateItemQuantityAsync(string userId, string productId, int quantity);
    Task<Cart> AddCartAsync(string userId);
    
}