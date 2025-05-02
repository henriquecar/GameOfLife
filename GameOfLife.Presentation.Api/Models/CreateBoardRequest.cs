namespace GameOfLife.Presentation.Api.Models;

/// <summary>
/// Represents the request body used to create a new Game of Life board.
/// Contains the initial state of the board as a jagged matrix of boolean values.
/// </summary>
/// <param name="InitialState">
/// The initial configuration of the board, where each inner list represents a row of cells.
/// `true` indicates a live cell, `false` indicates a dead cell.
/// </param>
public record CreateBoardRequest(List<List<bool>> InitialState)
{
    /// <summary>
    /// Initializes a new <see cref="CreateBoardRequest"/> with an empty matrix.
    /// </summary>
    public CreateBoardRequest() : this(new List<List<bool>>()) { }
}
