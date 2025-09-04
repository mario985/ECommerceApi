using System.Security.Cryptography;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUserRepository _userRepository;
    public RefreshTokenService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow
        };
    }

    public async Task<AuthModel> RefreshTokenAsync(string token)
    {
        AuthModel response = new AuthModel();
        var user = await _userRepository.FindByRefreshToken(token);
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
        response.IsAuthenticated = true;
        return response;
    }
}