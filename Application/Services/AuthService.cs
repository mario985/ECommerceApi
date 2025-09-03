
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ItokenService _tokenService;
    public AuthService(IUserRepository userRepository, ItokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    public async Task<IdentityResult> RegisterUser(User user, string Password)
    {
       return await _userRepository.AddAsync(user, Password);
       
       
       

    }
    public async Task<string?> LogInAsync(string Email, string Password)
    {
        var user = await _userRepository.GetByEmailAsync(Email);
        if (user == null)
        {
            return null;
        }
        var isPasswordValid = await _userRepository.CheckPassword(user, Password);
        if (!isPasswordValid)
        {
            return null;

        }
        var role = await _userRepository.GetRole(user);
        Console.WriteLine(role);
        var token = _tokenService.GenerateToken(user.UserName,role);
        return token;
        
    }
}