public interface IWishListService
{
    Task<WishListDto> GetWishListAsync(string userId);
    Task<WishListDto> AddToWishListAsync(AddToWishListDto addToWishListDto);
    Task<WishListDto> ClearWishListAsync(string userId);
    Task<WishListDto> RemoveFromWishListAsync(RemoveFromWishListDto removeFromWishListDto);
}