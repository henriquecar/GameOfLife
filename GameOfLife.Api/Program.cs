using GameOfLife.Api.Extensions;
using GameOfLife.Api.Mapper;
using GameOfLife.Api.Models;
using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using GameOfLife.Repository;
using GameOfLife.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//custom services
builder.Services.AddMapper();
builder.Services.AddRepository();
builder.Services.AddService();
builder.Services.AddScoped<IMapper<Board, BoardModel>, BoardMapper>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
