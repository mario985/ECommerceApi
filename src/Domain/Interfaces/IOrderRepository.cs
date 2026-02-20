public interface IorderRepository
{
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task RemoveAsync(Order order);
    Task<Order?> GetAsync(int id);
    Task<Order?> GetbyPaymentIntentIdAsync(string id);
    Task<List<Order>> GetAllAsync(string userId);
    Task<List<Order>> GetExpiredPendingAsync(DateTime utcNow, int take = 50);
}