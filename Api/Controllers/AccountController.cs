using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;


[Route("Api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    public AccountController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        RegisterCommand registerCommand = new RegisterCommand
        (register.Email, register.Password, register.UserName, register.Address);
        var result = await _mediator.Send(registerCommand);
        if (result.Succeeded)
        {
            return Ok("User Created Successfully");
        }
        var errors = result.Errors.Select(r => r.Description).ToList();
        return BadRequest(errors);

    }
    [HttpPost("LogIn")]
    public async Task<IActionResult> LogIn(LogInDto logInDto)
    {
        var result = await _authService.LogInAsync(logInDto.Email, logInDto.Password);
        if (result.IsAuthenticated == false)
        {

            return BadRequest(result.Message);
        }
        if (!string.IsNullOrEmpty(result.RefreshToken))
        {
            SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiration);
        }
         return Ok(result);

    }
    [HttpGet("test")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Test()
    {
        return Ok("Hello registerd Admin");

    }
    [HttpGet("token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["RefreshToken"];
        var result = await _authService.RefreshTokenAsync(refreshToken);
        if (!result.IsAuthenticated)
        {
            return BadRequest(result.Message);
        }
        if (!string.IsNullOrEmpty(result.RefreshToken))
        {
        SetRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiration);

        }
        return Ok(result);


    }
    [HttpPost("revokeToken")]
    public async Task<IActionResult> RevokeToken([FromBody]RevokeTokenDto revokeToken)
    {
        var token = revokeToken.Token ?? Request.Cookies["RefreshToken"];
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("token is required");
        }
        var result = await _authService.RevokeTokenAsync(token);
        if (!result)
        {
            return BadRequest("Token is invalid");
        }
        return Ok();

    }
    
    private void SetRefreshTokenCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime()
        };
        Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
    }

    
}