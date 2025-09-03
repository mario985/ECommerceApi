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
        if (result == null)
        {
            return BadRequest("Username or password is not correct");
        }
        else return Ok(result);

    }
    [HttpPost("test")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Test()
    {
        return Ok("Hello registerd Admin");

    }

    
}