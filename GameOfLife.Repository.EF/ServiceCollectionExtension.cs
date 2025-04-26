using GameOfLife.Repository.EF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Repository
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services) =>
            services
                .AddDbContext<GameDbContext>(opt =>
                    opt.UseSqlite("Data Source=gameoflife.db"))
                .AddScoped<IBoardRepository, BoardRepository>();
    }
}