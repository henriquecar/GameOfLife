using Microsoft.Extensions.DependencyInjection;

namespace GameOfLife.Common.Mapper
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services) =>
            services.AddScoped<IMapper<bool[,], List<List<bool>>>, BoolMatrixMapper>();
    }
}