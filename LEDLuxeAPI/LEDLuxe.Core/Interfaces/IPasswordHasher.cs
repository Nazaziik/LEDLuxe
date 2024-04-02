namespace LEDLuxe.Core.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool VerifyPasswordHash(string password, string passwordHash);
}