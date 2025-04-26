using GameOfLife.Model;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Repository.EF;

public class GameDbContext : DbContext
{
    public DbSet<Board> Boards { get; set; }

    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }
}
