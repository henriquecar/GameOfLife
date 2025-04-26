using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using GameOfLife.Repository;
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

    public Task SaveBoardAsync(Board board) => _repository.SaveBoardAsync(board);

    public Task<Board?> GetBoardAsync(Guid id) => _repository.GetBoardAsync(id);

    public async Task<Board?> NextGeneration(Guid id)
    {
        var board = await GetBoardAsync(id);
        if (board == null) return board;
        Next(board);

        await _repository.UpdateBoardAsync(board);

        return board;
    }

    public async Task<Board?> Advance(Guid id, int steps)
    {
        var board = await GetBoardAsync(id);
        if (board == null) return board;

        for (int i = 0; i < steps; i++)
            Next(board);

        await _repository.UpdateBoardAsync(board);
        return board;
    }

    public async Task<(Board?, bool)> FindFinalState(Guid id, int max)
    {
        var board = await GetBoardAsync(id);
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
                int alive = CountAlive(grid, r, c);
                if (grid[r, c])
                    next[r, c] = alive == 2 || alive == 3;
                else
                    next[r, c] = alive == 3;
            }
        }

        board.State = next;
        board.Step++;
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
}