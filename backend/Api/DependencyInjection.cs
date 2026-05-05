using MRAYAQ.Api.Endpoints;

namespace MRAYAQ.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        return services;
    }

    public static IEndpointRouteBuilder MapApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapHealthEndpoints();
        group.MapAuthEndpoints();
        group.MapContactEndpoints();
        group.MapCategoriesEndpoints();
        group.MapProductsEndpoints();
        group.MapOrdersEndpoints();

        return app;
    }
}
