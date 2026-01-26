using AutoMapper;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();
        CreateMap<OrderItem , OrderItemDto>();
        
    }
}