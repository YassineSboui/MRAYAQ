using MRAYAQ.Application.Contracts;
using MRAYAQ.Domain.Models;

namespace MRAYAQ.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (LoginRequest request, IAuthService service, CancellationToken cancellationToken) =>
        {
            var result = await service.LoginAsync(request, cancellationToken);
            return result is null ? Results.Unauthorized() : Results.Ok(result);
        });

        return app;
    }
}
