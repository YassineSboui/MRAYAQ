using MRAYAQ.Domain.Entities;

namespace MRAYAQ.Application.Contracts;

public interface IContactRepository
{
    Task AddAsync(ContactMessage message, CancellationToken cancellationToken = default);
}
