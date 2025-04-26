namespace GameOfLife.Api.Models;

public record BoardModel(Guid Id, List<List<bool>> State, int Step)
{
    public BoardModel() : this(Guid.NewGuid(), new List<List<bool>>(), 0) { }
}