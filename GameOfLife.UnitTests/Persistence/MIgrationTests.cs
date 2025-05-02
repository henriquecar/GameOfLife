using GameOfLife.Persistence.EF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GameOfLife.UnitTests.Persistence;

public class MigrationTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly GameDbContext _context;

    public MigrationTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<GameDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new GameDbContext(options);
    }

    [Fact]
    public void ShouldApplyAllMigrationsWithoutError()
    {
        // This will throw if migrations are invalid or incomplete
        _context.Database.Migrate();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}