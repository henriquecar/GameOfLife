using GameOfLife.Common.Mapper;
using GameOfLife.Domain;
using GameOfLife.Model;
using GameOfLife.Repository;
using System.Text.Json;

namespace GameOfLife.Services;

/// <summary>
/// Application service for handling Game of Life operations,
/// including board creation, evolution, and stabilization.
/// </summary>
public class GameOfLifeService : IGameOfLifeService
{
    private readonly IBoardRepository _repository;
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;
    private readonly BoardSimulator _simulator;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeService"/>.
    /// </summary>
    /// <param name="repository">Board repository abstraction for persistence.</param>
    /// <param name="matrixMapper">Matrix mapper for converting 2D arrays to jagged lists.</param>
    /// <param name="simulator">Domain simulator for applying Game of Life rules.</param>
    public GameOfLifeService(
        IBoardRepository repository,
        IMapper<bool[,], List<List<bool>>> matrixMapper,
        BoardSimulator simulator)
    {
        _repository = repository;
        _matrixMapper = matrixMapper;
        _simulator = simulator;
    }


    /// <summary>
    /// Creates and persists a new board with the provided initial state.
    /// </summary>
    /// <param name="initialState">The initial 2D boolean matrix.</param>
    /// <returns>The persisted <see cref="Board"/> entity.</returns>
    public Task<Board> SaveBoardAsync(bool[,] initialState)
    {
        var board = new Board(initialState);
        return _repository.SaveBoardAsync(board);
    }

    /// <summary>
    /// Advances a board by one generation according to the Game of Life rules.
    /// </summary>
    /// <param name="id">The unique board ID.</param>
    /// <returns>The updated board, or null if not found.</returns>
    public async Task<Board?> NextGeneration(Guid id)
    {
        var board = await _repository.GetBoardAsync(id);
        if (board == null) return board;

        Next(board);
        await _repository.UpdateBoardAsync(board);

        return board;
    }

    /// <summary>
    /// Advances a board by a specific number of steps.
    /// </summary>
    /// <param name="id">The unique board ID.</param>
    /// <param name="steps">The number of generations to simulate.</param>
    /// <returns>The updated board, or null if not found.</returns>
    public async Task<Board?> Advance(Guid id, int steps)
    {
        var board = await _repository.GetBoardAsync(id);
        if (board == null) return board;

        for (int i = 0; i < steps; i++)
            Next(board);

        await _repository.UpdateBoardAsync(board);
        return board;
    }

    /// <summary>
    /// Evolves a board until it reaches a stable state or the maximum number of steps is reached.
    /// </summary>
    /// <param name="id">The unique board ID.</param>
    /// <param name="max">The maximum number of steps to attempt.</param>
    /// <returns>A tuple containing the final board and a flag indicating whether a stable state was reached.</returns>
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

    /// <summary>
    /// Applies the Game of Life rules to evolve the board by one generation.
    /// </summary>
    /// <param name="board">The board to update.</param>
    private void Next(Board board)
    {
        board.State = _simulator.Next(board.State);
        board.Step++;
    }
}
