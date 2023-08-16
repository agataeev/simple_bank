namespace AccountManagement.Services.Abstraction;

public interface IUserService
{
    Task Login(string? username, string? pin);
    Task CreateUser(string? username, string? pin);
    Task UpdateUser(long id, string? username, string? pin);
}