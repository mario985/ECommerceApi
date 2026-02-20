using AutoMapper;

public class ShippingAddressProfile : Profile
{
    public ShippingAddressProfile()
    {
        CreateMap<ShippingAddress, ShippingAddressDto>();
        CreateMap<CreateShippingAddressDto, ShippingAddress>();
    }
}
