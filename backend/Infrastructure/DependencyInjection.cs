using Microsoft.EntityFrameworkCore;
using MRAYAQ.Application.Contracts;
using MRAYAQ.Infrastructure.Data;
using MRAYAQ.Infrastructure.Repositories;
using MRAYAQ.Infrastructure.Security;
using MRAYAQ.Infrastructure.Seeding;

namespace MRAYAQ.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = Environment.GetEnvironmentVariable("MRAYAQ_CONNECTION_STRING");
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string is missing. Set ConnectionStrings:Default or MRAYAQ_CONNECTION_STRING."
            );
        }

        services.AddDbContext<MrayaqDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddScoped<IAdminSeeder, AdminSeeder>();

        return services;
    }
}
