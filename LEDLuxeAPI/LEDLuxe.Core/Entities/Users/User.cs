using LEDLuxe.Core.Interfaces;
using System.Text.RegularExpressions;

namespace LEDLuxe.Core.Entities.Users;

public class User
{
    public Guid Id { get; private set; }

    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format.", nameof(Email));

            _email = value;
        }
    }

    private string _passwordHash;
    public string PasswordHash
    {
        get => _passwordHash;
        private set => _passwordHash = value ?? throw new ArgumentNullException(nameof(PasswordHash));
    }

    public User(string email, string hashedPassword)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = hashedPassword;
    }

    public void SetPassword(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("New password cannot be empty.", nameof(hashedPassword));

        PasswordHash = hashedPassword;
    }

    public void ChangePassword(string oldPassword, string newPassword, IPasswordHasher passwordHasher)
    {
        ArgumentNullException.ThrowIfNull(passwordHasher);

        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("New password cannot be empty.", nameof(newPassword));

        if (!passwordHasher.VerifyPasswordHash(oldPassword, _passwordHash))
            throw new ArgumentException("Old password is incorrect.", nameof(oldPassword));

        _passwordHash = passwordHasher.HashPassword(newPassword);
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}