using AccountManagement.Models.Entities;
using AccountManagement.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Repositories.Implementation;

public class TransactionRepo: ITransactionRepo
{
    
    private readonly ApplicationContext _db;

    public TransactionRepo(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<long?> Write(Transaction transaction)
    {
        var entity = await _db.Transaction.AddAsync(transaction);
        await _db.SaveChangesAsync();
        return entity.Entity?.Id;
    }
    
    public async Task<Transaction?> Read(long id)
    {
        return await _db.Transaction.FirstOrDefaultAsync(x=>x.Id == id);
    }
}