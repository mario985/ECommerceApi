public interface IWishListRepository
{
    Task AddWishListAsync(WishList wishList);
    Task<WishList?> GetByUserIdAsync(string userId);
    Task UpdateAsync(WishList wishList);
    Task<WishList?> GetByUserIdReadOnlyAsync(string UserId);
}