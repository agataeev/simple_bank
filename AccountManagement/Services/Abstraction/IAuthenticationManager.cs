using AccountManagement.Models.Responses;

namespace AccountManagement.Services.Abstraction;

public interface IAuthenticationManager
{
    Task<JwtExtendedResponse?> AuthenticateWithRefreshAsync(string login, string password);
    Task<JwtExtendedResponse?> RefreshAccessToken(string refreshToken);
}