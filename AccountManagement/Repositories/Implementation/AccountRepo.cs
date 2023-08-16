using AccountManagement.Models.Entities;
using AccountManagement.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Repositories.Implementation;

public class AccountRepo: IAccountRepo
{
    private readonly ApplicationContext _db;

    public AccountRepo(ApplicationContext db)
    {
        _db = db;
    }
    
    public async Task<Account> SaveAccount(Account account)
    {
        var entity = await _db.AddAsync(account);
        await _db.SaveChangesAsync();
        return entity.Entity;
    }
    
    public async Task<Account?> GetAccountById(long id)
    {
        return await _db.Account.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
    }
    
    public async Task UpdateAccount(Account account)
    {
        _db.Update(account);
        await _db.SaveChangesAsync();
    }
    
    public async Task DeleteAccount(Account account)
    {
        account.IsDeleted = true;
        _db.Update(account);
        await _db.SaveChangesAsync();
    }

    public async Task<Account[]> GetAccounts()
    {
        return await _db.Account.Where(x=>x.IsDeleted == false).ToArrayAsync();
    }
}