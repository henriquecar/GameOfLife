using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Domain;

/// <summary>
/// Represents the domain entity for a Game of Life board.
/// Encapsulates the current state of the board and its generation step.
/// </summary>
public class Board
{
    /// <summary>
    /// The unique identifier for the board instance.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The current state of the board as a 2D boolean matrix.
    /// Each value represents whether a cell is alive (<c>true</c>) or dead (<c>false</c>).
    /// </summary>
    public virtual bool[,] State { get; set; } = new bool[0, 0];

    /// <summary>
    /// The current generation (step count) of the board.
    /// </summary>
    public virtual int Step { get; set; } = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class.
    /// </summary>
    public Board() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with a predefined state.
    /// </summary>
    /// <param name="state">The initial matrix state of the board.</param>
    public Board(bool[,] state) => State = state;
}
