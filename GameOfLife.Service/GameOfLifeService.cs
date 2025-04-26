using GameOfLife.Common;
using GameOfLife.Model;
using GameOfLife.Repository;
using System.Text.Json;

namespace GameOfLife.Services;

public class GameOfLifeService
{
    private readonly IBoardRepository _repository;

    public GameOfLifeService(IBoardRepository repository)
    {
        _repository = repository;
    }

    public bool[,] NextGeneration(bool[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        var next = new bool[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int alive = CountAlive(grid, r, c);
                if (grid[r, c])
                    next[r, c] = alive == 2 || alive == 3;
                else
                    next[r, c] = alive == 3;
            }
        }
        return next;
    }

    public bool[,] Advance(bool[,] grid, int steps)
    {
        for (int i = 0; i < steps; i++)
            grid = NextGeneration(grid);
        return grid;
    }

    public (bool[,], bool) FindFinalState(bool[,] grid, int max)
    {
        var history = new HashSet<string>();
        for (int i = 0; i < max; i++)
        {
            var snapshot = JsonSerializer.Serialize(Serializer.ToJagged(grid));
            if (!history.Add(snapshot))
                return (grid, true);
            grid = NextGeneration(grid);
        }
        return (grid, false);
    }

    private int CountAlive(bool[,] grid, int r, int c)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int count = 0;

        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;
                int nr = r + dr, nc = c + dc;
                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && grid[nr, nc])
                    count++;
            }
        }
        return count;
    }

    public Task SaveBoardAsync(Board board) => _repository.SaveBoardAsync(board);

    public Task<Board?> GetBoardAsync(Guid id) => _repository.GetBoardAsync(id);

    public Task UpdateBoardAsync(Board board) => _repository.UpdateBoardAsync(board);
}