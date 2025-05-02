using GameOfLife.Domain;

namespace GameOfLife.Persistence;

/// <summary>
/// Defines the contract for board persistence operations.
/// Responsible for saving, retrieving, and updating <see cref="Board"/> entities.
/// This abstraction allows for different persistence strategies (e.g., EF Core, in-memory).
/// </summary>
public interface IBoardPersistence
{
    /// <summary>
    /// Persists a new board to the data store.
    /// </summary>
    /// <param name="board">The board entity to be saved.</param>
    /// <returns>The saved board with a unique identifier.</returns>
    Task<Board> SaveBoardAsync(Board board);

    /// <summary>
    /// Retrieves a board by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the board to retrieve.</param>
    /// <returns>The matching board, or <c>null</c> if not found.</returns>
    Task<Board?> GetBoardAsync(Guid id);

    /// <summary>
    /// Updates the state or metadata of an existing board.
    /// </summary>
    /// <param name="board">The board entity with updated state.</param>
    Task UpdateBoardAsync(Board board);
}
