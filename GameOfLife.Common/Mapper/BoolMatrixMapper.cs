using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Common.Mapper;

public class BoolMatrixMapper : IMapper<bool[,], List<List<bool>>>
{
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