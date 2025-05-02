using GameOfLife.Common.Mapper;
using GameOfLife.Domain;
using System.Text.Json;

namespace GameOfLife.Persistence.EF;

/// <summary>
/// EF Core implementation of <see cref="IBoardPersistence"/> for persisting Game of Life boards.
/// Stores only the alive cells in JSON format to optimize persistence.
/// </summary>
public class BoardPersistence : IBoardPersistence
{
    private readonly GameDbContext _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoardPersistence"/> class with the given database context.
    /// </summary>
    /// <param name="db">The EF Core database context.</param>
    public BoardPersistence(GameDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<Board> SaveBoardAsync(Board board)
    {
        var boardEF = new BoardEF(board.Id, board.State);
        var aliveCells = GetAliveCells(boardEF);
        boardEF.StateJson = JsonSerializer.Serialize(aliveCells);
        _db.Boards.Add(boardEF);
        await _db.SaveChangesAsync();
        return boardEF;
    }

    /// <inheritdoc />
    public async Task<Board?> GetBoardAsync(Guid id)
    {
        var boardEF = await _db.Boards.FindAsync(id);
        if (boardEF == null) return boardEF;

        var aliveCells = JsonSerializer.Deserialize<List<int[]>>(boardEF.StateJson);
        boardEF.State = BuildGridFromAliveCells(aliveCells!, boardEF.RowsCount, boardEF.ColsCount);
        return boardEF;
    }

    /// <inheritdoc />
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

    /// <summary>
    /// Extracts the coordinates of all alive cells from the given board.
    /// </summary>
    /// <param name="board">The board entity containing the current state matrix.</param>
    /// <returns>A list of integer arrays where each entry represents a live cell's (row, col).</returns>
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

    /// <summary>
    /// Reconstructs the full board matrix from a list of alive cell coordinates.
    /// </summary>
    /// <param name="aliveCells">A list of (row, col) pairs representing live cells.</param>
    /// <param name="rows">Total number of rows in the matrix.</param>
    /// <param name="cols">Total number of columns in the matrix.</param>
    /// <returns>A full boolean matrix with live cells marked as <c>true</c>.</returns>
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
