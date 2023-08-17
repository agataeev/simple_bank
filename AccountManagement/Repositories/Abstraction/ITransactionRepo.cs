using AccountManagement.Models.Entities;

namespace AccountManagement.Repositories.Abstraction;

public interface ITransactionRepo
{
    Task<long?> Write(Transaction transaction);
    Task<Transaction?> Read(long id);
}