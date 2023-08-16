using AccountManagement.Models.Entities;

namespace AccountManagement.Repositories.Abstraction;

public interface IUserRepo
{
    Task<User?> SaveUser(User user);
    Task<User?> GetUserById(long id);
    Task UpdateUser(User user);
    Task DeleteUser(User user);
    
    Task<User?> GetUserByUsername(string? username);
}