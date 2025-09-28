
public class InventoryRepository : IInventoryRepository
{
    public Task AddStockAsync(string productId, int quantity)
    {
        throw new NotImplementedException();
    }

    public Task<Inventory> GetByProductIdAsync(string productId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateStockAsync(string productId, int quantity)
    {
        throw new NotImplementedException();
    }
}