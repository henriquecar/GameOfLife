using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using System.Text.Json;

namespace GameOfLife.Repository.EF;
public class BoardRepository : IBoardRepository
{
    private readonly GameDbContext _db;

    public BoardRepository(GameDbContext db)
    {
        _db = db;
    }

    public async Task<Board> SaveBoardAsync(Board board)
    {
        var boardEF = new BoardEF(board.Id, board.State);
        var aliveCells = GetAliveCells(boardEF);
        boardEF.StateJson = JsonSerializer.Serialize(aliveCells);
        _db.Boards.Add(boardEF);
        await _db.SaveChangesAsync();
        return boardEF;
    }
    public async Task<Board?> GetBoardAsync(Guid id)
    {
        var boardEF = await _db.Boards.FindAsync(id);
        if (boardEF == null) return boardEF;


        var aliveCells = JsonSerializer.Deserialize<List<int[]>>(boardEF.StateJson);
        boardEF.State = BuildGridFromAliveCells(aliveCells!, boardEF.RowsCount, boardEF.ColsCount);
        return boardEF;
    }
    public async Task UpdateBoardAsync(Board board)
    {
        var boardEF = await _db.Boards.FindAsync(board.Id);
        if (boardEF == null) return;

        boardEF.Step = board.Step;
        boardEF.State = board.State;
        
        var aliveCells = GetAliveCells(boardEF);
        boardEF.StateJson = JsonSerializer.Serialize(aliveCells);

        _db.Boards.Update(boardEF);
        await _db.SaveChangesAsync();
    }
    private List<int[]> GetAliveCells(BoardEF board)
    {
        var aliveCells = new List<int[]>();

        for (int r = 0; r < board.RowsCount; r++)
        {
            for (int c = 0; c < board.ColsCount; c++)
            {
                if (board.State[r, c])
                {
                    aliveCells.Add([r, c]);
                }
            }
        }

        return aliveCells;
    }
    private bool[,] BuildGridFromAliveCells(List<int[]> aliveCells, int rows, int cols)
    {
        var grid = new bool[rows, cols];

        foreach (var rowCol in aliveCells)
        {
            var row = rowCol[0];
            var col = rowCol[1];
            if (row >= 0 && row < rows && col >= 0 && col < cols)
            {
                grid[row, col] = true;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Cell ({row}, {col}) is outside the grid size {rows}x{cols}.");
            }
        }

        return grid;
    }
}