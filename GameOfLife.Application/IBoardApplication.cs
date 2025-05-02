using GameOfLife.Domain;

namespace GameOfLife.Applications
{
    public interface IBoardApplication
    {
        Task<Board?> Advance(Guid id, int steps);
        Task<(Board?, bool)> FindFinalState(Guid id, int max);
        Task<Board?> NextGeneration(Guid id);
        Task<Board> SaveBoardAsync(bool[,] initialState);
    }
}