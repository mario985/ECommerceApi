using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;


[Route("Api/[controller]")]
[ApiController]
public class AccountController :ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    public AccountController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
    {
        var result = await _mediator.Send(registerCommand);
        if (result.Succeeded)
        {
            return Ok("User Created Successfully");
        }
        return BadRequest(result.Errors);
        
    }

    
}