public class WishListDto
{
    public int Id { set; get; }
    public ICollection<WishListItemDto>WishListItems{ set; get; }
}