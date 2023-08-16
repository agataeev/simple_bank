using AccountManagement.Models.Entities;
using AccountManagement.Models.Requests;

namespace AccountManagement.Services.Abstraction;

public interface IAccountService
{
    Task<string> CreateAccount(CreateAccountRequest request);
    Task EditAccount(EditAccountRequest request);
    Task DeleteAccount(long id);
    Task<Account> GetAccountById(long id);
    Task<Account[]> GetAccounts();
    
    Task AddBalance(BalanceRequest request);
    Task SubtractBalance(BalanceRequest request);
    Task TransferBalance(TransferBalanceRequest request);
}