using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<IdentityResult> RegisterUser(User user , string Password );
    Task<string> LogInAsync(string Email , string Password);
}