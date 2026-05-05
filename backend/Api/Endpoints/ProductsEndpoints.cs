using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Api.Endpoints;

public static class ProductsEndpoints
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (
            string? search,
            Guid? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            IProductService service,
            CancellationToken cancellationToken) =>
        {
            var items = await service.GetAsync(search, categoryId, minPrice, maxPrice, cancellationToken);
            return Results.Ok(items);
        });

        app.MapGet("/products/{id:guid}", async (Guid id, IProductService service, CancellationToken cancellationToken) =>
        {
            var item = await service.GetByIdAsync(id, cancellationToken);
            return item is null ? Results.NotFound() : Results.Ok(item);
        });

        app.MapPost("/products", async (CreateProductRequest request, IProductService service, CancellationToken cancellationToken) =>
        {
            var created = await service.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/products/{created.Id}", created);
        }).RequireAuthorization("Admin");

        app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductRequest request, IProductService service, CancellationToken cancellationToken) =>
        {
            var updated = await service.UpdateAsync(id, request, cancellationToken);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        }).RequireAuthorization("Admin");

        app.MapDelete("/products/{id:guid}", async (Guid id, IProductService service, CancellationToken cancellationToken) =>
        {
            var removed = await service.DeleteAsync(id, cancellationToken);
            return removed ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization("Admin");

        return app;
    }
}
