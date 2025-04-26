namespace GameOfLife.Api.Models;

public record BoardModel(List<List<bool>> InitialState)
{
    public BoardModel() : this(new List<List<bool>>()) { }
}