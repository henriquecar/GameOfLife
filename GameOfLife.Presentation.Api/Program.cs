using GameOfLife.Presentation.Api.Extensions;
using GameOfLife.Presentation.Api.Mapper;
using GameOfLife.Presentation.Api.Models;
using GameOfLife.Common.Mapper;
using GameOfLife.Domain;

namespace GameOfLife.Presentation.Api;

public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //custom services
        builder.Services.AddDomain();
        builder.Services.AddMapper();
        builder.Services.AddPersistence();
        builder.Services.AddApplication();
        builder.Services.AddScoped<IMapper<Board, BoardModel>, BoardMapper>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }

}
