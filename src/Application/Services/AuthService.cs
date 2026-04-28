
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ItokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    public AuthService(IUserRepository userRepository, ItokenService tokenService, IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }
    public async Task<IdentityResult> RegisterUser(User user, string Password)
    {
       return await _userRepository.AddAsync(user, Password);
    }
    public async Task<AuthModel> LogInAsync(string Email, string Password)
    {
        AuthModel response = new AuthModel { };
        var user = await _userRepository.GetByEmailWithTokensAsync(Email);
        if (user == null)
        {
            response.Message = "Email or password is not correct";
            response.IsAuthenticated = false;
            return response;
        }
        var isPasswordValid = await _userRepository.CheckPassword(user, Password);
        if (!isPasswordValid)
        {
            response.Message = "Email or password is not correct";
            response.IsAuthenticated = false;
            return response;

        }
        var role = await _userRepository.GetRole(user);
        Console.WriteLine(role);
        var token = _tokenService.GenerateToken(user.UserName, role , user);
         var NewrefreshToken = _refreshTokenService.GenerateRefreshToken();
        foreach (var tok in user.RefreshTokens.Where(t => t.IsActive))
        {
            tok.RevokedOn = DateTime.UtcNow;
        }
        user.RefreshTokens.Add(NewrefreshToken);
        await _userRepository.UpdateAsync(user);
        response.Message = "User Logged in successfully";
        response.Email = user.Email;
        response.Role = role;
        response.IsAuthenticated = true;
        response.UserName = user.UserName;
        response.Token = token;
         response.RefreshToken = NewrefreshToken.Token;
        response.RefreshTokenExpiration = NewrefreshToken.ExpiresOn;
        return response;
        
    }

    public async Task<AuthModel> RefreshTokenAsync(string token)
    {
        AuthModel response = new AuthModel();
        var user = await _userRepository.FindByRefreshTokenAsync(token);
        if (user == null)
        {
            response.IsAuthenticated = false;
            response.Message = "invalid token";
            return response;

        }
        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive)
        {
            response.IsAuthenticated = false;
            response.Message = "Inactive token";
            return response;
        }
        refreshToken.RevokedOn = DateTime.UtcNow;
        var newRefreshToken = _refreshTokenService.GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userRepository.UpdateAsync(user);
        var role = await _userRepository.GetRole(user);

        var newJwttoken = _tokenService.GenerateToken(user.UserName, role , user);
        response.IsAuthenticated = true;
        response.Email = user.Email;
        response.Role = role;
        response.IsAuthenticated = true;
        response.UserName = user.UserName;
        response.Token = newJwttoken;
        response.RefreshToken = newRefreshToken.Token;
        response.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
        return response;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userRepository.FindByRefreshTokenAsync(token);
        if (user == null)
            return false;
        var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
        if (!refreshToken.IsActive)
            return false;
        refreshToken.RevokedOn = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        return true;
        
    }

    public async Task<bool> RevokeAllTokensAsync(string token)
    {
        var user = await _userRepository.FindByRefreshTokenAsync(token);
        if (user == null)
            return false;
        foreach (var tok in user.RefreshTokens.Where(t => t.IsActive))
        {
            tok.RevokedOn = DateTime.UtcNow;
        }
        await _userRepository.UpdateAsync(user);
        return true;
    }
        public async Task<AuthModel> FindOrCreateByGoogleIdAsync(string email, string name, string googleId)
    {
        AuthModel response = new AuthModel();

        // Check if user already exists by GoogleId
        var user = await _userRepository.GetByGoogleIdAsync(googleId);

        if (user == null)
        {
            // First time login with Google — create the user
            user = new User
            {
                UserName = name,
                Email    = email,
                GoogleId = googleId,
            };

            var result = await _userRepository.AddGoogleUserAsync(user);

            if (!result.Succeeded)
            {
                response.IsAuthenticated = false;
                response.Message = "Failed to create user";
                return response;
            }
        }

        // From here it's identical to your LogInAsync
        var role = await _userRepository.GetRole(user);
        var token = _tokenService.GenerateToken(user.UserName, role, user);
        var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

        foreach (var tok in user.RefreshTokens.Where(t => t.IsActive))
        {
            tok.RevokedOn = DateTime.UtcNow;
        }

        user.RefreshTokens.Add(newRefreshToken);
        await _userRepository.UpdateAsync(user);

        response.IsAuthenticated        = true;
        response.Message                = "User logged in with Google successfully";
        response.Email                  = user.Email;
        response.UserName               = user.UserName;
        response.Role                   = role;
        response.Token                  = token;
        response.RefreshToken           = newRefreshToken.Token;
        response.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

        return response;
    }
}