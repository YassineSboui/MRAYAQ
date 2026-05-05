using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Api.Endpoints;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, IOrderService service, CancellationToken cancellationToken) =>
        {
            var created = await service.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/orders/{created.Id}", created);
        });

        app.MapGet("/orders/{id:guid}", async (Guid id, IOrderService service, CancellationToken cancellationToken) =>
        {
            var order = await service.GetByIdAsync(id, cancellationToken);
            return order is null ? Results.NotFound() : Results.Ok(order);
        });

        app.MapGet("/orders", async (int? take, IOrderService service, CancellationToken cancellationToken) =>
        {
            var size = take is > 0 and <= 50 ? take.Value : 10;
            var orders = await service.GetLatestAsync(size, cancellationToken);
            return Results.Ok(orders);
        });

        app.MapPut("/orders/{id:guid}/status", async (Guid id, StatusUpdateRequest request, IOrderService service, CancellationToken cancellationToken) =>
        {
            var updated = await service.UpdateStatusAsync(id, request.Status, cancellationToken);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        }).RequireAuthorization("Admin");

        return app;
    }
}

public record StatusUpdateRequest(string Status);
