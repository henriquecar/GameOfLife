namespace GameOfLife.Presentation.Api.Models;

/// <summary>
/// Represents the response returned when calculating the final state of a Game of Life board.
/// </summary>
/// <param name="Board">The final state of the board after evolution.</param>
/// <param name="Stable">Indicates whether a stable (unchanging) state was reached.</param>
public record FinalStateResult(BoardModel Board, bool Stable)
{
}
