using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public UserRepository(UserManager<User> userManager, AppDbContext dbContext)
    {
        _userManager = userManager;
        _context = dbContext;
    }

    public async Task<IdentityResult> AddAsync(User user, string password)
    {

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        return result;
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
    public async Task<User?> GetByEmailWithTokensAsync(string email)
    {
        return await _context.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _context.Users.Include(u => u.Cart).Include(u => u.WishList).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await _userManager.FindByNameAsync(name);
    }

    public async Task<IdentityResult> UpdateAsync(User user)
    {
        return await _userManager.UpdateAsync(user);

    }
    public async Task<string?> GetRole(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.FirstOrDefault();
       
        
    }

    public async Task<User?> FindByRefreshTokenAsync(string token)
    {
        return await _context.Users.Include(t => t.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
    }
    public async Task<bool> DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>u.GoogleId ==googleId);
    }

    public async Task<IdentityResult> AddGoogleUserAsync(User user)
    {
         var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user , "user");
        }
        return result;
    }
}
