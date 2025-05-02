using System;
using System.Net;
using System.Net.Http.Json;
using GameOfLife.Presentation.Api;
using GameOfLife.Presentation.Api.Models;
using GameOfLife.Persistence.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameOfLife.IntegrationTests;

public class BoardsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly GameDbContext _context;

    public BoardsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _scope = (factory.Services.GetRequiredService<IServiceScopeFactory>()).CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<GameDbContext>();
        // database is now shared across tests
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task Post_ShouldReturnOk_WithValidBoard()
    {
        var request = new CreateBoardRequest(new List<List<bool>>
        {
            new() { false, true, false },
            new() { false, true, false },
            new() { false, true, false }
        });

        var response = await _client.PostAsJsonAsync("/boards", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<BoardModel>();
        Assert.NotNull(result);
        Assert.Equal(3, result!.State.Count);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_WhenBoardTooSmall()
    {
        var request = new CreateBoardRequest(new List<List<bool>>
        {
            new() { true, false }
        });

        var response = await _client.PostAsJsonAsync("/boards", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetNext_ShouldReturnNextGeneration()
    {
        var create = new CreateBoardRequest(new List<List<bool>>
        {
            new() { false, true, false },
            new() { false, true, false },
            new() { false, true, false }
        });

        var postResponse = await _client.PostAsJsonAsync("/boards", create);
        var board = await postResponse.Content.ReadFromJsonAsync<BoardModel>();

        var nextResponse = await _client.GetAsync($"/boards/{board!.Id}/next");
        var nextBoard = await nextResponse.Content.ReadFromJsonAsync<BoardModel>();

        Assert.Equal(HttpStatusCode.OK, nextResponse.StatusCode);
        Assert.Equal(3, nextBoard!.State.Count);
    }

    [Fact]
    public async Task GetAdvance_ShouldReturnAdvancedBoard()
    {
        var create = new CreateBoardRequest(new List<List<bool>>
        {
            new() { false, true, false },
            new() { false, true, false },
            new() { false, true, false }
        });

        var postResponse = await _client.PostAsJsonAsync("/boards", create);
        var board = await postResponse.Content.ReadFromJsonAsync<BoardModel>();

        var steps = 5;
        var advanceResponse = await _client.GetAsync($"/boards/{board!.Id}/advance/{steps}");
        var result = await advanceResponse.Content.ReadFromJsonAsync<BoardModel>();

        Assert.Equal(HttpStatusCode.OK, advanceResponse.StatusCode);
        Assert.Equal(3, result!.State.Count);
        Assert.True(result.Step >= steps);
    }

    [Fact]
    public async Task GetFinal_ShouldReturnStableBoard()
    {
        var create = new CreateBoardRequest(new List<List<bool>>
        {
            new() { false, false, false },
            new() { false, true,  true },
            new() { false, true,  true }
        });

        var postResponse = await _client.PostAsJsonAsync("/boards", create);
        var board = await postResponse.Content.ReadFromJsonAsync<BoardModel>();

        var finalResponse = await _client.GetAsync($"/boards/{board!.Id}/final");
        finalResponse.EnsureSuccessStatusCode();

        var result = await finalResponse.Content.ReadFromJsonAsync<FinalStateResult>();

        Assert.True(result!.Stable);
        Assert.Equal(3, result.Board.State.Count);
    }
}