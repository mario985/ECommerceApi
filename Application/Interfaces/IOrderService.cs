public interface IOrderService
{
    Task<PlaceOrderResult> PlaceOrderAsync(string userId);
    Task<OrderDto> GetAsync(int id);
    Task<List<OrderDto>> GetByUserIdAsync(string userId);
    
}