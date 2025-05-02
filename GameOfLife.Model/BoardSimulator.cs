namespace GameOfLife.Domain;

/// <summary>
/// Domain-level simulator responsible for applying the rules of Conway's Game of Life.
/// Provides a pure and stateless method to compute the next generation of a board.
/// </summary>
public class BoardSimulator
{
    /// <summary>
    /// Computes the next generation of the given board state.
    /// </summary>
    /// <param name="current">The current 2D boolean grid representing the board.</param>
    /// <returns>A new 2D boolean grid representing the next generation.</returns>
    public bool[,] Next(bool[,] current)
    {
        int rows = current.GetLength(0);
        int cols = current.GetLength(1);
        var next = new bool[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int alive = CountAliveNeighbors(current, r, c);
                next[r, c] = current[r, c]
                    ? (alive == 2 || alive == 3)
                    : (alive == 3);
            }
        }

        return next;
    }

    /// <summary>
    /// Counts how many alive neighbors surround a given cell.
    /// </summary>
    private int CountAliveNeighbors(bool[,] grid, int row, int col)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int aliveCount = 0;

        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;

                int nr = row + dr, nc = col + dc;
                if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && grid[nr, nc])
                    aliveCount++;
            }
        }

        return aliveCount;
    }
}
