using MediatR;
using Microsoft.AspNetCore.Identity;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
{
    private readonly IAuthService _authService;
    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.FullName,
        };
         return await _authService.RegisterUser(user, request.Password);
       

    }
}