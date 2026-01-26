public interface IOrderService
{
    Task<OrderDto> PlaceOrderAsync(string userId);
    Task<OrderDto> GetAsync(int id);
    Task<List<OrderDto>> GetByUserIdAsync(string userId);
    
}