
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
        await _appDbContext.WishList.AddAsync(wishList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<WishList?> GetByUserIdAsync(string userId)
    {
        return await _appDbContext.WishList.Include(ws =>ws.WishListItems).FirstOrDefaultAsync(ws => ws.UserId == userId);
    }

    public async Task<WishList?> GetByUserIdReadOnlyAsync(string UserId)
    {
        return await _appDbContext
        .WishList
        .Include(ws => ws.WishListItems)
        .AsNoTracking()
        .FirstOrDefaultAsync(ws => ws.UserId == UserId);
                                         
    }

    public async Task UpdateAsync(WishList wishList)
    {
        _appDbContext.WishList.Update(wishList);
        await _appDbContext.SaveChangesAsync();
    }
}