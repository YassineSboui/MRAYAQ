using Microsoft.EntityFrameworkCore;
using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;
using MRAYAQ.Infrastructure.Data;

namespace MRAYAQ.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly MrayaqDbContext _dbContext;

    public AdminRepository(MrayaqDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _dbContext.AdminUsers.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

    public async Task AddAsync(AdminUser admin, CancellationToken cancellationToken = default)
    {
        _dbContext.AdminUsers.Add(admin);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
