using AccountManagement.Models.Entities;

namespace AccountManagement.Repositories.Abstraction;

public interface IAccountRepo
{
    Task<Account> SaveAccount(Account account);
    Task<Account?> GetAccountById(long id);
    Task UpdateAccount(Account account);
    Task DeleteAccount(Account account);
    
    Task<Account[]> GetAccounts();
}