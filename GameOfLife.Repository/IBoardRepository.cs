using GameOfLife.Model;

namespace GameOfLife.Repository;
public interface IBoardRepository
{
    Task<Board> SaveBoardAsync(Board board);
    Task<Board?> GetBoardAsync(Guid id);
    Task UpdateBoardAsync(Board board);
}