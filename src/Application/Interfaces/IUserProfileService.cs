using System.Threading.Tasks;

public interface IUserProfileService
{
    Task<UserProfileDto> GetUserProfileAsync(string userId);
    Task<bool> UpdateUserProfileAsync(string userId, UpdateUserProfileDto updateDto);
    Task<bool> DeleteUserAsync(string userId);
}