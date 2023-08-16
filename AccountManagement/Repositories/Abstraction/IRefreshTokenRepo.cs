using AccountManagement.Models.Entities;

namespace AccountManagement.Repositories.Abstraction;

public interface IRefreshTokenRepo
{
    Task<RefreshToken?> SaveRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
}