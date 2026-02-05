using MediatR;
using Microsoft.AspNetCore.Identity;

public record RegisterCommand
(
string Email,
string Password,
string FullName
)
:IRequest<IdentityResult>;