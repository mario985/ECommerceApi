public interface IShippingAddressRepository
{
    Task<List<ShippingAddress>> GetByUserIdAsync(string userId);

    Task AddAsync(ShippingAddress address);

    Task ClearDefaultAsync(string userId);
}
