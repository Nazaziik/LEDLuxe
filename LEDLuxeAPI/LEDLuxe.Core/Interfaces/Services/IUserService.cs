using LEDLuxe.Core.Entities.Users;

namespace LEDLuxe.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> AuthenticateUserAsync(string email, string password);
        Task<User> CreateUserAsync(string email, string password);
        Task DeleteUserAsync(Guid userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid id);
        Task UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task UpdateUserAsync(User user);
    }
}