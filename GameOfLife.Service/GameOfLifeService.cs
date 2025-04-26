using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using GameOfLife.Repository;
using System.Reflection;
using System.Text.Json;

namespace GameOfLife.Services;

public class GameOfLifeService
{
    private readonly IBoardRepository _repository;
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;

    public GameOfLifeService(IBoardRepository repository, IMapper<bool[,], List<List<bool>>> matrixMapper)
    {
        _repository = repository;
        _matrixMapper = matrixMapper;
    }

    public Task<Board> SaveBoardAsync(bool[,] initialState) {
        var board = new Board(initialState);
        return _repository.SaveBoardAsync(board);
    }

    public async Task<Board?> NextGeneration(Guid id)
    {
        var board = await _repository.GetBoardAsync(id);
        if (board == null) return board;
        Next(board);

        await _repository.UpdateBoardAsync(board);

        return board;
    }

    public async Task<Board?> Advance(Guid id, int steps)
    {
        var board = await _repository.GetBoardAsync(id);
        if (board == null) return board;

        for (int i = 0; i < steps; i++)
            Next(board);

        await _repository.UpdateBoardAsync(board);
        return board;
    }

    public async Task<(Board?, bool)> FindFinalState(Guid id, int max)
    {
        var board = await _repository.GetBoardAsync(id);
        if (board == null) return (board, false);

        var history = new HashSet<string>();
        for (int i = 0; i < max; i++)
        {
            var snapshot = JsonSerializer.Serialize(_matrixMapper.To(board.State));
            if (!history.Add(snapshot))
            {
                await _repository.UpdateBoardAsync(board);
                return (board, true);
            }
            Next(board);
        }
        return (board, false);
    }

    private void Next(Board board)
    {
        var grid = board.State;
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        var next = new bool[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int alive = CountAliveNeighbors(grid, r, c);
                if (grid[r, c])
                    next[r, c] = alive == 2 || alive == 3;
                else
                    next[r, c] = alive == 3;
            }
        }

        board.State = next;
        board.Step++;
    }


    private int CountAliveNeighbors(bool[,] grid, int row, int col)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int aliveCount = 0;

        for (int relativeRowOffset = -1; relativeRowOffset <= 1; relativeRowOffset++)
        {
            for (int relativeColOffset = -1; relativeColOffset <= 1; relativeColOffset++)
            {
                if (IsCurrentCell(relativeRowOffset, relativeColOffset))
                    continue;

                int neighborRow = row + relativeRowOffset;
                int neighborCol = col + relativeColOffset;

                if (IsInsideGrid(neighborRow, neighborCol, rows, cols) && grid[neighborRow, neighborCol])
                    aliveCount++;
            }
        }

        return aliveCount;
    }

    private static bool IsCurrentCell(int relativeRowOffset, int relativeColOffset)
    {
        return relativeRowOffset == 0 && relativeColOffset == 0;
    }

    private bool IsInsideGrid(int row, int col, int totalRows, int totalCols)
    {
        return row >= 0 && row < totalRows && col >= 0 && col < totalCols;
    }
}