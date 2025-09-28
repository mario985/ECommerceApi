public interface IInventoryRepository
{
    Task AddStockAsync(string productId, int quantity);
    Task UpdateStockAsync(string productId, int quantity);
    Task<Inventory> GetByProductIdAsync(string productId);
    
}