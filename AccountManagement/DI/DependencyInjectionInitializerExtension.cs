using AccountManagement.Models.Entities;
using AccountManagement.Repositories.Abstraction;
using AccountManagement.Repositories.Implementation;
using AccountManagement.Services.Abstraction;
using AccountManagement.Services.Implementation;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.DI;

public static class DependencyInjectionInitializerExtension
{
    public static void InitializeDependencies(this IServiceCollection services,  IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Db"));
        });

        services.AddTransient<IUserRepo, UserRepo>();
        services.AddTransient<IAccountRepo, AccountRepo>();
        services.AddTransient<IRefreshTokenRepo, RefreshTokenRepo>();
        
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAccountService, AccountService>();

        services.AddTransient<IAuthenticationManager, AuthenticationManager>();
    }
}