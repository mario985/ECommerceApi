using MediatR;
using Microsoft.AspNetCore.Identity;

public record RegisterCommand
(
string Email,
string Password,
string FullName,
string Address,
string Role
)
:IRequest<IdentityResult>;