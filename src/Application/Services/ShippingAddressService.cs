using AutoMapper;

public class ShippingAddressService : IShippingAddressService
{
    private readonly IShippingAddressRepository _shippingAddressRepository;
    private readonly IMapper _mapper;

    public ShippingAddressService(IShippingAddressRepository shippingAddressRepository, IMapper mapper)
    {
        _shippingAddressRepository = shippingAddressRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ShippingAddressDto>> GetAddressesAsync(string userId)
    {
        var addresses = await _shippingAddressRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ShippingAddressDto>>(addresses);
    }

    public async Task<ShippingAddressDto> AddAddressAsync(string userId, CreateShippingAddressDto dto)
    {
        var existingAddresses = await _shippingAddressRepository.GetByUserIdAsync(userId);

        var address = _mapper.Map<ShippingAddress>(dto);
        address.UserId = userId;

        if (!existingAddresses.Any())
        {
            address.IsDefault = true;
        }
        else if (dto.IsDefault)
        {
            await _shippingAddressRepository.ClearDefaultAsync(userId);
            address.IsDefault = true;
        }

        await _shippingAddressRepository.AddAsync(address);
        return _mapper.Map<ShippingAddressDto>(address);
    }
}
