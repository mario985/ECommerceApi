public interface IShippingAddressService
{
    Task<IEnumerable<ShippingAddressDto>> GetAddressesAsync(string userId);

    Task<ShippingAddressDto> AddAddressAsync(string userId, CreateShippingAddressDto dto);
}
