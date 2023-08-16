using AccountManagement.Models.Entities;
using AccountManagement.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Repositories.Implementation;

public class UserRepo: IUserRepo
{
    
    private readonly ApplicationContext _db;

    public UserRepo(ApplicationContext db)
    {
        _db = db;
    }
    
    public async Task<User?> SaveUser(User user)
    {
        var entity = await _db.User.AddAsync(user);
        await _db.SaveChangesAsync();
        return entity.Entity;
    }
    
    public async Task<User?> GetUserById(long id)
    {
        return await _db.User.Where(x => x != null && x.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task UpdateUser(User user)
    {
        _db.Update(user);
        await _db.SaveChangesAsync();
    }
    
    public async Task DeleteUser(User user)
    {
        user.IsDeleted = true;
        _db.User.Update(user);
        await _db.SaveChangesAsync();
    }

    public Task<User?> GetUserByUsername(string? username)
    {
        return _db.User.Where(x => x.Username == username).FirstOrDefaultAsync();;
    }
}