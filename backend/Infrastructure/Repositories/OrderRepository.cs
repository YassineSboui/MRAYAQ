using Microsoft.EntityFrameworkCore;
using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;
using MRAYAQ.Infrastructure.Data;

namespace MRAYAQ.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly MrayaqDbContext _dbContext;

    public OrderRepository(MrayaqDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<List<Order>> GetLatestAsync(int take, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
