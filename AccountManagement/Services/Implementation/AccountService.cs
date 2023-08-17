using AccountManagement.Models.Common;
using AccountManagement.Models.Entities;
using AccountManagement.Models.Requests;
using AccountManagement.Repositories.Abstraction;
using AccountManagement.Services.Abstraction;

namespace AccountManagement.Services.Implementation;

public class AccountService: IAccountService
{
    
    private readonly IAccountRepo _accountRepo;
    private readonly ITransactionRepo _transactionRepo;
    private readonly Serilog.ILogger _logger;

    public AccountService(IAccountRepo accountRepo, Serilog.ILogger logger, ITransactionRepo transactionRepo)
    {
        _accountRepo = accountRepo;
        _logger = logger;
        _transactionRepo = transactionRepo;
    }

    public async Task<string> CreateAccount(CreateAccountRequest request)
    {
        var account = new Account
        {
            Type    = 1,
            Active = true,
            Number = Guid.NewGuid().ToString(),
            Currency = request.Currency,
            Balance = request.Balance,
        };
        await _accountRepo.SaveAccount(account);
        _logger.Information("CreateAccount number: {number}", account.Number);
        return account.Number;
    }

    public async Task<Account> GetAccountById(long id)
    {
        _logger.Information("GetAccountById ID: {id}", id);
        var account = await _accountRepo.GetAccountById(id);
        if (account == null)
        {
            throw new Exception("Account not found");
        }
        return account;
    }
    
    public async Task<Account[]> GetAccounts()
    {
        _logger.Information("GetAccounts");
        var accounts = await _accountRepo.GetAccounts();
        return accounts;
    }
    
    public async Task EditAccount(EditAccountRequest request)
    {
        _logger.Information("EditAccount ID: {id}", request.Id);
        var account = await _accountRepo.GetAccountById(request.Id);
        if (account == null)
        {
            throw new Exception("Account not found");
        }
        account.Currency = request.Currency;
        account.Balance = request.Balance;
        account.Active = request.Active;
        await _accountRepo.UpdateAccount(account);
    }

    public async Task DeleteAccount(long id)
    {
        _logger.Information("DeleteAccount ID: {id}", id);
        var account = await _accountRepo.GetAccountById(id);
        if (account == null)
        {
            throw new Exception("Account not found");
        }
        await _accountRepo.DeleteAccount(account);
    }
    
    public async Task AddBalance(BalanceRequest request)
    {
        _logger.Information("AddBalance ID: {id}", request.Id);
        var account = await _accountRepo.GetAccountById(request.Id);
        if (account == null)
        {
            throw new Exception("Account not found");
        }

        if (account.Active)
        {
            account.Balance += request.Amount;
        }else
        {
            throw new Exception("Account is not active");
        }
        
        await _accountRepo.UpdateAccount(account);

        var transaction = new Transaction
        {
            Type = (int) TransactionType.Debit,
            AccountId = account.Id,
            Amount = request.Amount,
            Balance = account.Balance,
            Created = DateTime.Now.ToUniversalTime(),
        };
        await _transactionRepo.Write(transaction);
    }
    
    public async Task SubtractBalance(BalanceRequest request)
    {
        _logger.Information("SubtractBalance ID: {id}", request.Id);
        var account = await _accountRepo.GetAccountById(request.Id);
        if (account == null)
        {
            throw new Exception("Account not found");
        }

        if (request.Amount < account.Balance)
        {
            account.Balance -= request.Amount;    
        }
        else if (!account.Active)
        {
            throw new Exception("Account is not active");
        }
        else
        {
            throw new Exception("Not enough money");
        }
        
        await _accountRepo.UpdateAccount(account);
        
        var transaction = new Transaction
        {
            Type = (int) TransactionType.Credit,
            AccountId = account.Id,
            Amount = request.Amount,
            Balance = account.Balance,
            Created = DateTime.Now.ToUniversalTime(),
        };
        await _transactionRepo.Write(transaction);
    }
    
    public async Task TransferBalance(TransferBalanceRequest request)
    {
        _logger.Information("TransferBalance fromId: {fromId} toId: {toId}", request.FromId, request.ToId);
        var fromAccount = await _accountRepo.GetAccountById(request.FromId);
        if (fromAccount == null)
        {
            throw new Exception("Account not found");
        }

        var toAccount = await _accountRepo.GetAccountById(request.ToId);
        if (toAccount == null)
        {
            throw new Exception("Account not found");
        }

        switch (fromAccount.Currency)
        {
            case 1:
                if (toAccount.Currency != 1)
                {
                    throw new Exception("Currency mismatch");
                }
                break;
            case 2:
                if (toAccount.Currency != 2)
                {
                    throw new Exception("Currency mismatch");
                }
                break;
            case 3:
                if (toAccount.Currency != 3)
                {
                    throw new Exception("Currency mismatch");
                }
                break;
            default:
                throw new Exception("Currency mismatch");
        }
        
        if (request.Amount < fromAccount.Balance)
        {
            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;
        }else if (!fromAccount.Active)
        {
            throw new Exception("Account is not active");
        }
        else
        {
            throw new Exception("Not enough money");
        }
        
        await _accountRepo.UpdateAccount(fromAccount);
        await _accountRepo.UpdateAccount(toAccount);
        
        var transaction = new Transaction
        {
            Type = (int) TransactionType.Transfer,
            AccountId = fromAccount.Id,
            Amount = request.Amount,
            Balance = fromAccount.Balance,
            Created = DateTime.Now.ToUniversalTime(),
            ToAccountId = toAccount.Id
        };
        await _transactionRepo.Write(transaction);
    }
}