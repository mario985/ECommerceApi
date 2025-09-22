using AutoMapper;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartDto>();
        CreateMap<CartItem, CartItemDto>();
    }
}