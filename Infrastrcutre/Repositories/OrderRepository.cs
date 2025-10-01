
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

    public async Task<Order?>GetAsync(int id)
    {
        return await _appDbContext.Order.FirstOrDefaultAsync(o => o.Id == id);
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
}