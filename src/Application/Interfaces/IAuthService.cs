using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<IdentityResult> RegisterUser(User user, string Password);
    Task<AuthModel> LogInAsync(string Email, string Password);
    Task<AuthModel> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
    Task<bool> RevokeAllTokensAsync(string token);
}