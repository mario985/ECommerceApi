using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
public class TokenService : ItokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration configuration)
    {
        _config = configuration;
    }
    public string GenerateToken(string UserName, string Role , User user)
    {
        var claims = new List<Claim>
     {
            new Claim(JwtRegisteredClaimNames.Email, UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, Role),
            new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString()),
            new Claim("google_id", user.GoogleId ?? string.Empty)


        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JWT:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var lifetimeMinutes = double.Parse(_config["JWT:AccessTokenLifetimeMinutes"]);
        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(lifetimeMinutes), 
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}