public interface IorderRepository
{
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task RemoveAsync(Order order);
    Task<Order?> GetAsync(int id);
    Task<List<Order>> GetAllAsync(string userId);
}