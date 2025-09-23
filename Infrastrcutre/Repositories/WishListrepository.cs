
using Microsoft.EntityFrameworkCore;

public class WishListRepository : IWishListRepository
{
    private readonly AppDbContext _appDbContext;
    public WishListRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task AddWishListAsync(WishList wishList)
    {
        await _appDbContext.wishList.AddAsync(wishList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<WishList?> GetByUserIdAsync(string userId)
    {
        return await _appDbContext.wishList.Include(ws =>ws.WishListItems).FirstOrDefaultAsync(ws => ws.UserId == userId);
    }

    public async Task<WishList?> GetByUserIdReadOnlyAsync(string UserId)
    {
        return await _appDbContext
        .wishList
        .Include(ws => ws.WishListItems)
        .AsNoTracking()
        .FirstOrDefaultAsync(ws => ws.UserId == UserId);
                                         
    }

    public async Task UpdateAsync(WishList wishList)
    {
        _appDbContext.wishList.Update(wishList);
        await _appDbContext.SaveChangesAsync();
    }
}