using Microsoft.Extensions.DependencyInjection;

namespace GameOfLife.Services;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddService(this IServiceCollection services) =>
        services.AddScoped<GameOfLifeService>();
}