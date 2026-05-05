using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;
using MRAYAQ.Infrastructure.Data;

namespace MRAYAQ.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly MrayaqDbContext _dbContext;

    public ContactRepository(MrayaqDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ContactMessage message, CancellationToken cancellationToken = default)
    {
        _dbContext.ContactMessages.Add(message);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
