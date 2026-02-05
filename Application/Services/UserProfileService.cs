// Application/Services/UserProfileService.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserProfileService : IUserProfileService
{
    private readonly IUserRepository _userRepository;

    public UserProfileService( IUserRepository userRepository)
    {
     
        _userRepository = userRepository;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return null;

        return new UserProfileDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<bool> UpdateUserProfileAsync(string userId, UpdateUserProfileDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        // Update user properties
        user.UserName = updateDto.UserName ?? user.UserName;
        user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;

        var result = await _userRepository.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;
        return await _userRepository.DeleteUserAsync(user);
        
    }
}