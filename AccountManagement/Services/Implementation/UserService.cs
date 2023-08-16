using AccountManagement.Managers.Abstraction;
using AccountManagement.Models.Entities;
using AccountManagement.Repositories.Abstraction;
using AccountManagement.Services.Abstraction;

namespace AccountManagement.Services.Implementation;

public class UserService : IUserService
{
    private readonly Serilog.ILogger _logger;
    private readonly IUserRepo _userRepo;

    // private readonly IJwtTokenManager _jwtTokenManager;
    // private readonly IJwtTokenManager _jwtTokenManager;

    public UserService(Serilog.ILogger logger, IUserRepo userRepo)
    {
        _logger = logger;
        _userRepo = userRepo;
        // _jwtTokenManager = jwtTokenManager;
    }

    public async Task Login(string? username, string? pin)
    {
        if (!await IsValidLogin(username, pin))
        {
            throw new Exception("Invalid login");
        }
    }

    private async Task<bool> IsValidLogin(string? username, string? pin)
    {
        var user = await _userRepo.GetUserByUsername(username);
        return user != null && user.Pin == pin;
    }

    private async Task<string> GenerateToken(string? username)
    {
        var token = await GenerateTokenAsync(username);
        return token;
    }

    private async Task<string> GenerateTokenAsync(string? username)
    {
        return await Task.FromResult("token");
    }

    public async Task CreateUser(string? username, string? pin)
    {
        var user = new User
        {
            Username = username,
            Pin = pin,
            IsDeleted = false,
            Rights = 1
        };
        await _userRepo.SaveUser(user);
    }

    public async Task UpdateUser(long id, string? username, string? pin)
    {
        var user = await _userRepo.GetUserById(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        user.Username = username;
        user.Pin = pin;
        await _userRepo.UpdateUser(user);
    }
}