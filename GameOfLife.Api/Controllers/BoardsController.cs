using GameOfLife.Api.Models;
using GameOfLife.Common;
using GameOfLife.Model;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BoardsController : ControllerBase
{
    private readonly GameOfLifeService _service;

    public BoardsController(GameOfLifeService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BoardModel model)
    {
        var board = new Board(Serializer.To2D(model.InitialState));
        await _service.SaveBoardAsync(board);
        return Ok(new { id = board.Id });
    }

    [HttpGet("{id}/next")]
    public async Task<IActionResult> Next(Guid id)
    {
        var board = await _service.GetBoardAsync(id);
        if (board == null) return NotFound();
        var next = _service.NextGeneration(board.CurrentState);
        board.CurrentState = next;
        await _service.UpdateBoardAsync(board);
        return Ok(new BoardModel(Serializer.ToJagged(next)));
    }

    [HttpGet("{id}/advance/{steps:int}")]
    public async Task<IActionResult> Steps(Guid id, int steps)
    {
        var board = await _service.GetBoardAsync(id);
        if (board == null) return NotFound();
        var next = _service.Advance(board.CurrentState, steps);
        board.CurrentState = next;
        await _service.UpdateBoardAsync(board);
        return Ok(new BoardModel(Serializer.ToJagged(next)));
    }

    [HttpGet("{id}/final")]
    public async Task<IActionResult> Final(Guid id, int? max)
    {
        var board = await _service.GetBoardAsync(id);
        if (board == null) return NotFound();
        var (final, stable) = _service.FindFinalState(board.CurrentState, max ?? 1000);
        if (!stable) return BadRequest("Board did not reach a stable state");
        board.CurrentState = final;
        await _service.UpdateBoardAsync(board);
        return Ok(new BoardModel(Serializer.ToJagged(final)));
    }
}