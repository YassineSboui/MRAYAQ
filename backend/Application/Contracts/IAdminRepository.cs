using MRAYAQ.Domain.Entities;

namespace MRAYAQ.Application.Contracts;

public interface IAdminRepository
{
    Task<AdminUser?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task AddAsync(AdminUser admin, CancellationToken cancellationToken = default);
}
