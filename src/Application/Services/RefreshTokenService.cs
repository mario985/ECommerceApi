using System.Security.Cryptography;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public RefreshTokenService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var lifetimeDays = double.Parse(_configuration["JWT:RefreshTokenLifetimeDays"]);
        
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresOn = DateTime.UtcNow.AddDays(lifetimeDays),
            CreatedOn = DateTime.UtcNow
        };
    }

  
}