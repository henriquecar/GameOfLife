using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using System.Text.Json;

namespace GameOfLife.Repository.EF;
public class BoardRepository : IBoardRepository
{
    private readonly GameDbContext _db;
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;

    public BoardRepository(GameDbContext db, IMapper<bool[,], List<List<bool>>> matrixMapper)
    {
        _db = db;
        _matrixMapper = matrixMapper;
    }

    public async Task<Board> SaveBoardAsync(Board board)
    {
        var boardEF = new BoardEF(board.Id, board.State);
        boardEF.StateJson = JsonSerializer.Serialize(_matrixMapper.To(board.State));
        _db.Boards.Add(boardEF);
        await _db.SaveChangesAsync();
        return boardEF;
    }
    public async Task<Board?> GetBoardAsync(Guid id)
    {
        var boardEF = await _db.Boards.FindAsync(id);
        if (boardEF == null) return boardEF;

        var currentState = JsonSerializer.Deserialize<List<List<bool>>>(boardEF.StateJson);
        boardEF.State = _matrixMapper.From(currentState!);
        return boardEF;
    }
    public async Task UpdateBoardAsync(Board board)
    {
        var boardEF = await _db.Boards.FindAsync(board.Id);
        if (boardEF == null) return;

        boardEF.Step = board.Step;
        boardEF.StateJson = JsonSerializer.Serialize(_matrixMapper.To(board.State));

        _db.Boards.Update(boardEF);
        await _db.SaveChangesAsync();
    }
}