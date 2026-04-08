namespace Api.Features.AccountManagementContext.Shared.Services;

public class PasswordService
{
    // TODO: replace with proper hash later
    public string HashPassword(string password)
    {
        return password;
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return password == passwordHash;
    }
}