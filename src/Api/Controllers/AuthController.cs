using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    public IAuthService _authservice;
    public AuthController(IAuthService authService)
    {
        _authservice = authService;
    }
    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback))
        };
        return Challenge(properties , GoogleDefaults.AuthenticationScheme);
    }
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return Unauthorized("Google authentication failed");
        var email = result.Principal.FindFirstValue(ClaimTypes.Email);
        var name = result.Principal.FindFirstValue(ClaimTypes.Name);
        var googleId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _authservice.FindOrCreateByGoogleIdAsync(email! , name! , googleId!);
        if(!response.IsAuthenticated)
            return Unauthorized(response.Message);
        return Ok(response);
    }
}