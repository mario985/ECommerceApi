using Microsoft.EntityFrameworkCore;

public class InventoryRepository : IInventoryRepository
{
    private readonly AppDbContext _appDbContext;

    public InventoryRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Inventory inventory)
    {
        await _appDbContext.Inventories.AddAsync(inventory);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Inventory?> GetByProductIdAsync(string productId)
    {
        return await _appDbContext.Inventories
            .FirstOrDefaultAsync(p => p.ProductId == productId);
    }
    
    public async Task UpdateAsync(Inventory inventory)
    {
        _appDbContext.Inventories.Update(inventory);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Inventory inventory)
    {
        _appDbContext.Inventories.Remove(inventory);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> ReduceQuantityAsync(string productId, int reduceBy)
    {
        var rowsAffected = await _appDbContext.Database.ExecuteSqlInterpolatedAsync($@"
        UPDATE Inventories
        SET Quantity = Quantity - {reduceBy}
        WHERE ProductId = {productId}
          AND Quantity >= {reduceBy}
        ");
        return rowsAffected > 0 ;
    }


      public async Task<bool> TryApplyStockTransitionAsync(string productId, int qty, StockTransition transition)
    {
        // IMPORTANT: This is atomic because it is ONE SQL UPDATE with a guard in WHERE.

        int rowsAffected = transition switch
        {
            StockTransition.Reserve => await _appDbContext.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE Inventories
                SET Reserved = Reserved + {qty}
                WHERE ProductId = {productId}
                  AND (OnHand - Reserved) >= {qty};
            "),

            StockTransition.Commit => await _appDbContext.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE Inventories
                SET OnHand = OnHand - {qty},
                    Reserved = Reserved - {qty}
                WHERE ProductId = {productId}
                  AND Reserved >= {qty};
            "),

            StockTransition.Release => await _appDbContext.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE Inventories
                SET Reserved = Reserved - {qty}
                WHERE ProductId = {productId}
                  AND Reserved >= {qty};
            "),

            _ => throw new ArgumentOutOfRangeException(nameof(transition), transition, null)
        };

        return rowsAffected > 0;
    }
}
