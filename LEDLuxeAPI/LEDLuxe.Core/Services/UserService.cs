using LEDLuxe.Core.Entities.Users;
using LEDLuxe.Core.Interfaces;
using LEDLuxe.Core.Interfaces.Repositories;
using LEDLuxe.Core.Interfaces.Services;

namespace LEDLuxe.Core.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IUserService, IUserService
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));

    public async Task<User> CreateUserAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Email and password are required.");

        if (await _userRepository.GetByEmailAsync(email) != null)
            throw new InvalidOperationException("A user with this email already exists.");

        var hashedPassword = _passwordHasher.HashPassword(password);
        var user = new User(email, hashedPassword);

        await _userRepository.AddAsync(user);

        return user;
    }

    public async Task<bool> AuthenticateUserAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return false;

        return _passwordHasher.VerifyPasswordHash(password, user.PasswordHash);
    }

    public async Task UpdatePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("User not found.");

        if (!_passwordHasher.VerifyPasswordHash(currentPassword, user.PasswordHash))
            throw new ArgumentException("Current password is incorrect.");

        user.ChangePassword(currentPassword, newPassword, _passwordHasher);
        await _userRepository.UpdateAsync(user);
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (!User.IsValidEmail(user.Email))
            throw new ArgumentException("Invalid email format.", nameof(user.Email));

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await _userRepository.DeleteAsync(userId);
    }
}