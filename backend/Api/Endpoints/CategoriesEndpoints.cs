using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Api.Endpoints;

public static class CategoriesEndpoints
{
    public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/categories", async (ICategoryService service, CancellationToken cancellationToken) =>
        {
            var items = await service.GetAsync(cancellationToken);
            return Results.Ok(items);
        });

        app.MapPost("/categories", async (CreateCategoryRequest request, ICategoryService service, CancellationToken cancellationToken) =>
        {
            var created = await service.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/categories/{created.Id}", created);
        }).RequireAuthorization("Admin");

        app.MapPut("/categories/{id:guid}", async (Guid id, UpdateCategoryRequest request, ICategoryService service, CancellationToken cancellationToken) =>
        {
            var updated = await service.UpdateAsync(id, request, cancellationToken);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        }).RequireAuthorization("Admin");

        app.MapDelete("/categories/{id:guid}", async (Guid id, ICategoryService service, CancellationToken cancellationToken) =>
        {
            var removed = await service.DeleteAsync(id, cancellationToken);
            return removed ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization("Admin");

        return app;
    }
}
