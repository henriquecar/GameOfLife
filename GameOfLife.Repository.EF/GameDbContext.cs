using GameOfLife.Model;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Repository.EF;

/// <summary>
/// EF Core database context for the Game of Life application.
/// Provides access to persistent storage for board entities.
/// </summary>
public class GameDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the set of persisted Game of Life boards.
    /// This maps to the database table for <see cref="BoardEF"/> entities.
    /// </summary>
    public DbSet<BoardEF> Boards { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameDbContext"/> class.
    /// </summary>
    /// <param name="options">The configuration options for the DbContext.</param>
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options) { }
}
