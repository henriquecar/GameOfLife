using GameOfLife.Model;

namespace GameOfLife.Services
{
    public interface IGameOfLifeService
    {
        Task<Board?> Advance(Guid id, int steps);
        Task<(Board?, bool)> FindFinalState(Guid id, int max);
        Task<Board?> NextGeneration(Guid id);
        Task<Board> SaveBoardAsync(bool[,] initialState);
    }
}