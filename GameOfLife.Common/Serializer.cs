using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Common;

public class Serializer
{
    public static bool[,] To2D(List<List<bool>> jagged)
    {
        int rows = jagged.Count;
        int cols = jagged[0].Count;
        var result = new bool[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                result[i, j] = jagged[i][j];
        return result;
    }

    public static bool[,] To2D(string json)
    {
        var jagged = JsonSerializer.Deserialize<List<List<bool>>>(json)!;
        return To2D(jagged);
    }

    public static List<List<bool>> ToJagged(bool[,] array)
    {
        var result = new List<List<bool>>();
        for (int i = 0; i < array.GetLength(0); i++)
        {
            var row = new List<bool>();
            for (int j = 0; j < array.GetLength(1); j++)
                row.Add(array[i, j]);
            result.Add(row);
        }
        return result;
    }
}