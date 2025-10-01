public interface IInventoryService
{
    Task AddStockAsync(string productId, int quantity);
    Task ChangeStockAsync(string productId, int quantity);
    Task ReduceQuantityAsync(string productId, int quantity);
    Task<Inventory> GetInventoryAsync(string ProductId);
}