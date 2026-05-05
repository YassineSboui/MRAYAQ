namespace MRAYAQ.Domain.Models;

public record CreateOrderItemRequest(Guid ProductId, int Quantity);

public record CreateOrderRequest(
    string CustomerName,
    string Email,
    string Phone,
    string Address,
    string City,
    string Notes,
    List<CreateOrderItemRequest> Items
);

public record OrderItemDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record OrderDto(
    Guid Id,
    string CustomerName,
    string Email,
    string Phone,
    string Address,
    string City,
    string Notes,
    decimal TotalAmount,
    string Status,
    DateTimeOffset CreatedAt,
    List<OrderItemDto> Items
);
