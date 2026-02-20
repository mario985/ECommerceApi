using Microsoft.EntityFrameworkCore;

public class ShippingAddressRepository : IShippingAddressRepository
{
    private readonly AppDbContext _appDbContext;

    public ShippingAddressRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<ShippingAddress>> GetByUserIdAsync(string userId)
    {
        return await _appDbContext
            .Set<ShippingAddress>()
            .Where(a => a.UserId == userId)
            .AsNoTracking()
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.Id)
            .ToListAsync();
    }

    public async Task AddAsync(ShippingAddress address)
    {
        await _appDbContext.Set<ShippingAddress>().AddAsync(address);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task ClearDefaultAsync(string userId)
    {
        var defaults = await _appDbContext
            .Set<ShippingAddress>()
            .Where(a => a.UserId == userId && a.IsDefault)
            .ToListAsync();

        if (!defaults.Any())
        {
            return;
        }

        foreach (var address in defaults)
        {
            address.IsDefault = false;
        }

        await _appDbContext.SaveChangesAsync();
    }
}
