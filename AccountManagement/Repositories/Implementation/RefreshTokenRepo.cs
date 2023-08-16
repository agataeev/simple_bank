using AccountManagement.Models.Entities;
using AccountManagement.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Repositories.Implementation;

public class RefreshTokenRepo: IRefreshTokenRepo
{
    private readonly ApplicationContext _db;

    public RefreshTokenRepo(ApplicationContext db)
    {
        _db = db;
    }
    
    public async Task<RefreshToken?> SaveRefreshToken(RefreshToken refreshToken)
    {
        if (await _db.RefreshToken.AnyAsync(x => x.Id == refreshToken.Id))
        {
            _db.RefreshToken.Update(refreshToken);
            await _db.SaveChangesAsync();
            return refreshToken;
        }
        var entity = await _db.RefreshToken.AddAsync(refreshToken);
        await _db.SaveChangesAsync();
        return entity.Entity;
    }

    public Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
    {
        return _db.RefreshToken.Where(x => x.Value == token).FirstOrDefaultAsync();
    }
}