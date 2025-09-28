
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    public CartRepository(AppDbContext appDbContext, IUserRepository userRepository, IProductRepository productRepository)
    {
        _dbContext = appDbContext;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }
    public async Task<Cart?> AddToCartAsync(string userId, string productId, int quantity)
    {
        var cart = await GetCartByUserIdAsync(userId);
        var product = await _productRepository.GetById(productId);
        if (cart == null)
            return cart;
        var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                Cart = cart,
                ProductId = productId,
                Product = product,
                Quantity = quantity
            };
            cart.CartItems.Add(cartItem);
        }
        await _dbContext.SaveChangesAsync();
        return cart;
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart == null)
            return false;
        cart.CartItems.Clear();
        var result = await _dbContext.SaveChangesAsync();
        return true;


    }

    public async Task<UpdateCartResult> DeleteFromCartAsync(string userId, string productId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart == null)
            return new UpdateCartResult { Cart = cart, IsSuccess = false, Message = "User not found" };

        var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (item == null)
            return new UpdateCartResult { Cart = cart, IsSuccess = false, Message = "Item not found in cart" };

        cart.CartItems.Remove(item);
        await _dbContext.SaveChangesAsync();

        return new UpdateCartResult { Cart = cart, IsSuccess = true };
    }



    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
        var cart = await _dbContext.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
      
        return cart;


    }

    public async Task<UpdateCartResult> UpdateItemQuantityAsync(string userId, string productId, int quantity)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart == null)
            return new UpdateCartResult { Cart = cart, IsSuccess = false, Message = "User not found" };
        var itemToUpdate = cart.CartItems.FirstOrDefault(ct => ct.ProductId == productId);
        if (itemToUpdate == null)
            return new UpdateCartResult { Cart = cart, IsSuccess = false, Message = "Item not found" };
        itemToUpdate.Quantity = quantity;
        var result = await _dbContext.SaveChangesAsync();
        return new UpdateCartResult { Cart = cart, IsSuccess = true, };
    }
    public async Task<Cart> AddCartAsync(string userId)
    {
        var cart = new Cart { UserId = userId };
        _dbContext.Cart.Add(cart);
        await _dbContext.SaveChangesAsync();
        return cart;

    }
}