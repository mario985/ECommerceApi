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
    public string GenerateToken(string UserName, string Role)
    {
        var claims = new List<Claim>
     {
            new Claim(JwtRegisteredClaimNames.Sub, UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, Role)

        };
    
        

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JWT:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15), 
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}