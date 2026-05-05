namespace MRAYAQ.Infrastructure.Seeding;

public interface IAdminSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
