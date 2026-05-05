using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Contracts;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<OrderDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<OrderDto>> GetLatestAsync(int take, CancellationToken cancellationToken = default);
    Task<OrderDto?> UpdateStatusAsync(Guid id, string status, CancellationToken cancellationToken = default);
}
