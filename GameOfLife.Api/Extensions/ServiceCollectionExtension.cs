using GameOfLife.Repository.EF;
using Microsoft.EntityFrameworkCore;
using GameOfLife.Repository;
using GameOfLife.Common.Mapper;
using GameOfLife.Services;

namespace GameOfLife.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepository(this IServiceCollection services) =>
        services
            .AddDbContext<GameDbContext>(opt =>
                opt.UseSqlite("Data Source=gameoflife.db"))
            .AddScoped<IBoardRepository, BoardRepository>();
    public static IServiceCollection AddMapper(this IServiceCollection services) =>
        services.AddScoped<IMapper<bool[,], List<List<bool>>>, BoolMatrixMapper>();
    public static IServiceCollection AddService(this IServiceCollection services) =>
        services.AddScoped<GameOfLifeService>();
}