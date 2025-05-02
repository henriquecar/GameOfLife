using GameOfLife.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOfLife.Repository.EF
{
    /// <summary>
    /// Entity Framework Core-compatible representation of the <see cref="Board"/> domain entity.
    /// Handles serialization of 2D matrices and stores additional metadata for persistence.
    /// </summary>
    public class BoardEF : Board
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoardEF"/> class.
        /// </summary>
        public BoardEF() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardEF"/> class with a specified ID and state.
        /// Also sets the internal row and column counts based on the matrix dimensions.
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <param name="currentState">The 2D array representing the board state.</param>
        public BoardEF(Guid id, bool[,] currentState)
        {
            Id = id;
            State = currentState;
            RowsCount = currentState.GetLength(0);
            ColsCount = currentState.GetLength(1);
        }

        /// <summary>
        /// The in-memory matrix representation of the board.
        /// Marked as [NotMapped] so it won't be persisted directly by EF Core.
        /// </summary>
        [NotMapped]
        public override bool[,] State { get; set; } = new bool[0, 0];

        /// <summary>
        /// A JSON string that represents the serialized board state.
        /// Used to persist the 2D matrix in a format compatible with EF Core.
        /// </summary>
        public string StateJson { get; set; } = string.Empty;

        /// <summary>
        /// The number of rows in the matrix (used for deserialization and reconstruction).
        /// </summary>
        public int RowsCount { get; set; } = 0;

        /// <summary>
        /// The number of columns in the matrix (used for deserialization and reconstruction).
        /// </summary>
        public int ColsCount { get; set; } = 0;
    }
}
