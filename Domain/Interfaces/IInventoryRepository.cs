public interface IInventoryRepository
{
    Task AddAsync(Inventory inventory);
    Task UpdateAsync(Inventory inventory);
    Task<Inventory?> GetByProductIdAsync(string productId);
    Task RemoveAsync(Inventory inventory);
    Task<bool> ReduceQuantityAsync(string productId , int reduceBy);
    
}