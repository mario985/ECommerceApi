using AutoMapper;

public class wishListProfile : Profile
{
    public wishListProfile()
    {
        CreateMap<WishList, WishListDto>();
        CreateMap<WishListItem , WishListItemDto>();
    }
}