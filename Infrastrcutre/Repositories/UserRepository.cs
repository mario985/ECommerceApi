using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
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

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
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
}
