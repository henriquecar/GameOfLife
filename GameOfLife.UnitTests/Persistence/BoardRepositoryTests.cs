using GameOfLife.Domain;
using GameOfLife.Persistence.EF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GameOfLife.UnitTests.Persistence;

public class BoardRepositoryTests : IDisposable
{
    private readonly GameDbContext _context;
    private readonly BoardPersistence _repository;
    private readonly SqliteConnection _connection;

    public BoardRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<GameDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new GameDbContext(options);
        _context.Database.EnsureCreated();

        _repository = new BoardPersistence(_context);
    }

    [Fact]
    public async Task SaveAndGetBoard_ShouldPersistAndRetrieve()
    {
        var state = new bool[,]
        {
            { false, true },
            { true, false }
        };

        var saved = await _repository.SaveBoardAsync(new Board(state));
        var retrieved = await _repository.GetBoardAsync(saved.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(saved.Id, retrieved!.Id);
        Assert.Equal(saved.State, retrieved.State);
    }

    [Fact]
    public async Task UpdateBoard_ShouldPersistStepAndStateChanges()
    {
        var state = new bool[,]
        {
            { true, false },
            { false, true }
        };

        var board = await _repository.SaveBoardAsync(new Board(state));
        board.Step = 5;
        board.State[0, 0] = false;

        await _repository.UpdateBoardAsync(board);
        var updated = await _repository.GetBoardAsync(board.Id);

        Assert.Equal(5, updated!.Step);
        Assert.False(updated.State[0, 0]);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}