using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Entities;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;

    public OrderService(IOrderRepository orders, IProductRepository products)
    {
        _orders = orders;
        _products = products;
    }

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("Order must contain at least one item.");
        }

        var productLookup = new Dictionary<Guid, Product>();
        var productIds = request.Items.Select(item => item.ProductId).Distinct();
        foreach (var productId in productIds)
        {
            var product = await _products.GetByIdAsync(productId, cancellationToken);
            if (product is null)
            {
                throw new InvalidOperationException("Product not found.");
            }

            productLookup[productId] = product;
        }

        var order = new Order
        {
            CustomerName = request.CustomerName.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone.Trim(),
            Address = request.Address.Trim(),
            City = request.City.Trim(),
            Notes = request.Notes?.Trim() ?? string.Empty,
            CreatedAt = DateTimeOffset.UtcNow
        };

        foreach (var item in request.Items)
        {
            var product = productLookup[item.ProductId];
            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = Math.Max(1, item.Quantity),
                UnitPrice = product.Price,
                Product = product
            };

            order.Items.Add(orderItem);
            order.TotalAmount += orderItem.UnitPrice * orderItem.Quantity;
        }

        await _orders.AddAsync(order, cancellationToken);

        return ToDto(order);
    }

    public async Task<OrderDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(id, cancellationToken);
        return order is null ? null : ToDto(order);
    }

    public async Task<List<OrderDto>> GetLatestAsync(int take, CancellationToken cancellationToken = default)
    {
        var orders = await _orders.GetLatestAsync(take, cancellationToken);
        return orders.Select(ToDto).ToList();
    }

    public async Task<OrderDto?> UpdateStatusAsync(Guid id, string status, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            return null;
        }

        order.Status = status.ToLower();
        await _orders.UpdateAsync(order, cancellationToken);

        return ToDto(order);
    }

    private static OrderDto ToDto(Order order)
    {
        var items = order.Items.Select(item => new OrderItemDto(
            item.ProductId,
            item.Product?.Name ?? string.Empty,
            item.Quantity,
            item.UnitPrice
        )).ToList();

        return new OrderDto(
            order.Id,
            order.CustomerName,
            order.Email,
            order.Phone,
            order.Address,
            order.City,
            order.Notes,
            order.TotalAmount,
            order.Status,
            order.CreatedAt,
            items
        );
    }
}
