public interface IInventoryService
{
    Task AddStockAsync(string productId, int quantity);
    Task ChangeStockAsync(string productId, int quantity);
    Task<Inventory> GetInventoryAsync(string ProductId);
    public Task ReserveStockAsync(string productId, int reduceBy);

    public Task CommitReservedStockAsync(string productId, int reduceBy);

    public Task ReleaseReservedStockAsync(string productId, int reduceBy);

}