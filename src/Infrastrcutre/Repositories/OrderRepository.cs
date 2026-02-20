
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IorderRepository
{
    private readonly AppDbContext _appDbContext;
    public OrderRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;
    public async Task AddAsync(Order order)
    {
        await _appDbContext.Order.AddAsync(order);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<Order>> GetAllAsync(string userId)
    {
        return await _appDbContext.Order.Include(o => o.OrderItems).Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task<Order?> GetAsync(int id)
    {
        return await _appDbContext.Order.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetbyPaymentIntentIdAsync(string id)
    {
        return await _appDbContext.Order.Include(o=>o.OrderItems).FirstOrDefaultAsync(o =>o.StripePaymentIntentId == id);
    }

    public async Task RemoveAsync(Order order)
    {
        _appDbContext.Order.Remove(order);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _appDbContext.Order.Update(order);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task<List<Order>> GetExpiredPendingAsync(DateTime utcNow, int take = 50)
{
    return await _appDbContext.Order
        .Include(o => o.OrderItems)
        .Where(o => o.Status == OrderStatus.PendingPayment &&
                    o.ExpiresAt <= utcNow)
        .OrderBy(o => o.ExpiresAt)
        .Take(take)
        .ToListAsync();
}

    
}