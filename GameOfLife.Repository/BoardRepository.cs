using GameOfLife.Model;

namespace GameOfLife.Repository.EF;
public class BoardRepository : IBoardRepository
{
    private readonly GameDbContext _db;
    public BoardRepository(GameDbContext db) => _db = db;
    public async Task SaveBoardAsync(Board board)
    {
        _db.Boards.Add(board);
        await _db.SaveChangesAsync();
    }
    public Task<Board?> GetBoardAsync(Guid id) => _db.Boards.FindAsync(id).AsTask();
    public async Task UpdateBoardAsync(Board board)
    {
        _db.Boards.Update(board);
        await _db.SaveChangesAsync();
    }
}