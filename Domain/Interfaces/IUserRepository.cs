using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public interface IUserRepository
{
    Task<IdentityResult> AddAsync(User user, string Password);
    Task<User?> GetByIdAsync(string Id);
    Task<User?> GetByEmailAsync(string Email);
    Task<User?> GetByNameAsync(string Name);
    Task<IdentityResult> UpdateAsync(User user);
    Task<bool> CheckPassword(User user, string Password);
}