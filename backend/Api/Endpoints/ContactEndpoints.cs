using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Api.Endpoints;

public static class ContactEndpoints
{
    public static IEndpointRouteBuilder MapContactEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/contact", async (ContactRequest request, IContactService service, CancellationToken cancellationToken) =>
        {
            var result = await service.HandleAsync(request, cancellationToken);
            return result.Success
                ? Results.Ok(result.Response)
                : Results.BadRequest(new { message = result.ErrorMessage });
        });

        return app;
    }
}
