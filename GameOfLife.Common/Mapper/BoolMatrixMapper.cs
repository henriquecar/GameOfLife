using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Common.Mapper;

/// <summary>
/// Maps between a 2D array (<c>bool[,]</c>) and a jagged list representation (<c>List&lt;List&lt;bool&gt;&gt;</c>).
/// Useful for serialization compatibility and API transport formats.
/// </summary>
public class BoolMatrixMapper : IMapper<bool[,], List<List<bool>>>
{
    /// <summary>
    /// Converts a jagged list of booleans to a multidimensional boolean array.
    /// </summary>
    /// <param name="entity">The jagged list representing the board state.</param>
    /// <returns>A multidimensional array equivalent to the input structure.</returns>
    public bool[,] From(List<List<bool>> entity)
    {
        int rows = entity.Count;
        int cols = entity[0].Count;
        var result = new bool[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[i, j] = entity[i][j];
        return result;
    }

    /// <summary>
    /// Converts a multidimensional boolean array to a jagged list.
    /// </summary>
    /// <param name="entity">The 2D array representing the board state.</param>
    /// <returns>A jagged list suitable for JSON serialization or transport.</returns>
    public List<List<bool>> To(bool[,] entity)
    {
        var result = new List<List<bool>>();
        for (int i = 0; i < entity.GetLength(0); i++)
        {
            var row = new List<bool>();
            for (int j = 0; j < entity.GetLength(1); j++)
                row.Add(entity[i, j]);
            result.Add(row);
        }
        return result;
    }
}
