namespace GameOfLife.Api.Models;

public record FinalStateResult(BoardModel Board, bool Stable)
{
}