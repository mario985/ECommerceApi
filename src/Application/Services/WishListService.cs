
using AutoMapper;

public class WishListService : IWishListService
{
    private readonly IWishListRepository _wishListRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public WishListService(IWishListRepository wishListRepository, IProductRepository productRepository, IMapper mapper)
    {
        _wishListRepository = wishListRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<WishListDto> AddToWishListAsync(AddToWishListDto addToWishListDto)
    {
        var wishlist = await _wishListRepository.GetByUserIdAsync(addToWishListDto.UserId);
        if (wishlist == null)
        {
            wishlist = new WishList { UserId = addToWishListDto.UserId, WishListItems = new List<WishListItem>() };
            await _wishListRepository.AddWishListAsync(wishlist);
        }
        if (!wishlist.WishListItems.Any(ws => ws.ProductId == addToWishListDto.ProductId))
        {
            try
            {
                await _productRepository.GetById(addToWishListDto.ProductId);
            }
            catch
            {
                throw new KeyNotFoundException("product not found");
            }
                wishlist.WishListItems.Add(new WishListItem { ProductId = addToWishListDto.ProductId, WishListId = wishlist.Id });
            await _wishListRepository.UpdateAsync(wishlist);

        }
        return await MapWishListtoDtoAsync(wishlist);
    }

    public async Task<WishListDto> ClearWishListAsync(string userId)
    {
        var wishlist = await _wishListRepository.GetByUserIdAsync(userId);
        if (wishlist == null)
        {
            throw new KeyNotFoundException("wishlist not found");
        }
        wishlist.WishListItems.Clear();
        await _wishListRepository.UpdateAsync(wishlist);
        return await MapWishListtoDtoAsync(wishlist);
    }

    public async Task<WishListDto> GetWishListAsync(string userId)
    {
        var wishlist = await _wishListRepository.GetByUserIdReadOnlyAsync(userId);
        if (wishlist == null)
        {
            wishlist = new WishList { UserId = userId };
            await _wishListRepository.AddWishListAsync(wishlist);
        }
        return await MapWishListtoDtoAsync(wishlist);

    }

    public async Task<WishListDto> RemoveFromWishListAsync(RemoveFromWishListDto removeFromWishListDto)
    {
        var wishlist = await _wishListRepository.GetByUserIdAsync(removeFromWishListDto.UserId);
        if (wishlist == null)
        {
            throw new KeyNotFoundException("wishlist not found");
        }
        var item = wishlist.WishListItems.FirstOrDefault(ws => ws.ProductId == removeFromWishListDto.ProductId);
        if (item == null)
        {
            throw new KeyNotFoundException("item not found");
        }
        wishlist.WishListItems.Remove(item);
        await _wishListRepository.UpdateAsync(wishlist);
        return await MapWishListtoDtoAsync(wishlist);
    }
    private async Task<WishListDto> MapWishListtoDtoAsync(WishList wishList)
    {
          if (wishList == null) return null;
        if (!wishList.WishListItems.Any())
            return _mapper.Map<WishListDto>(wishList);
        var productIds = wishList.WishListItems.Select(p => p.ProductId).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds);
        var productDic = products.ToDictionary(p => p.Id);
        var wishListDto = _mapper.Map<WishListDto>(wishList);
        foreach (var itemDto in wishListDto.WishListItems)
        {
            if (productDic.TryGetValue(itemDto.ProductId, out var product))
            {
                itemDto.Product = _mapper.Map<ProductDto>(product);

            }
        }
        return wishListDto ;
    
    }
}