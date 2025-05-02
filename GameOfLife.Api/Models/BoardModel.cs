namespace GameOfLife.Api.Models;

/// <summary>
/// API-facing representation of a Game of Life board.
/// Encapsulates board identity, current state, and generation step.
/// </summary>
/// <param name="Id">The unique identifier of the board.</param>
/// <param name="State">The current state of the board as a jagged boolean matrix.</param>
/// <param name="Step">The current generation (iteration count) of the board.</param>
public record BoardModel(Guid Id, List<List<bool>> State, int Step)
{
    /// <summary>
    /// Initializes a new <see cref="BoardModel"/> with default values:
    /// a new GUID, an empty matrix, and step = 0.
    /// </summary>
    public BoardModel() : this(Guid.NewGuid(), new List<List<bool>>(), 0) { }
}
