using Microsoft.Extensions.Configuration;
using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;

namespace MRAYAQ.Infrastructure.Seeding;

public class AdminSeeder : IAdminSeeder
{
    private readonly IConfiguration _configuration;
    private readonly IAdminRepository _admins;
    private readonly IPasswordHasher _hasher;

    public AdminSeeder(IConfiguration configuration, IAdminRepository admins, IPasswordHasher hasher)
    {
        _configuration = configuration;
        _admins = admins;
        _hasher = hasher;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var username = _configuration["AdminSeed:Username"];
        var password = _configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var existing = await _admins.GetByUsernameAsync(username, cancellationToken);
        if (existing is not null)
        {
            return;
        }

        var (hash, salt) = _hasher.HashPassword(password);
        var admin = new AdminUser
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _admins.AddAsync(admin, cancellationToken);
    }
}
