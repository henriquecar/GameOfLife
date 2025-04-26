namespace GameOfLife.Api.Models;

public record CreateBoardRequest(List<List<bool>> InitialState)
{
    public CreateBoardRequest() : this(new List<List<bool>>()) { }
}