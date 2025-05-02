using GameOfLife.Repository.EF;
using Microsoft.EntityFrameworkCore;
using GameOfLife.Repository;
using GameOfLife.Common.Mapper;
using GameOfLife.Services;
using GameOfLife.Domain;

namespace GameOfLife.Api.Extensions;

/// <summary>
/// Extension methods for registering services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers the Entity Framework Core DbContext and board repository implementation.
    /// Uses SQLite as the backing store.
    /// </summary>
    /// <param name="services">The service collection to add dependencies to.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services
            .AddDbContext<GameDbContext>(opt =>
                opt.UseSqlite("Data Source=gameoflife.db"))
            .AddScoped<IBoardRepository, BoardRepository>();

    /// <summary>
    /// Registers a custom matrix mapper used to convert between jagged and multidimensional boolean arrays.
    /// </summary>
    /// <param name="services">The service collection to add dependencies to.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddMapper(this IServiceCollection services) =>
        services.AddScoped<IMapper<bool[,], List<List<bool>>>, BoolMatrixMapper>();

    /// <summary>
    /// Registers the main Game of Life service containing the board evolution logic.
    /// </summary>
    /// <param name="services">The service collection to add dependencies to.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddService(this IServiceCollection services) =>
        services.AddScoped<IGameOfLifeService, GameOfLifeService>();

    /// <summary>
    /// Registers the main Game of Life domain.
    /// </summary>
    /// <param name="services">The service collection to add dependencies to.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services.AddSingleton<BoardSimulator>();
}
