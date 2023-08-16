namespace AccountManagement.Managers.Abstraction;

public interface IJwtTokenManager
{
    Task<string> GenerateToken(string? username, int expireMinutes = 60);
    Task<string> ValidateToken(string token);
}