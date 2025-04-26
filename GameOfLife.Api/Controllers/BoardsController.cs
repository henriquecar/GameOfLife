using GameOfLife.Api.Models;
using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BoardsController : ControllerBase
{
    private readonly GameOfLifeService _service;
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;

    public BoardsController(GameOfLifeService service, IMapper<bool[,], List<List<bool>>> matrixMapper)
    {
        _service = service;
        _matrixMapper = matrixMapper;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BoardModel model)
    {
        var board = new Board(_matrixMapper.From(model.InitialState));
        await _service.SaveBoardAsync(board);
        return Ok(new { id = board.Id });
    }

    [HttpGet("{id}/next")]
    public async Task<IActionResult> Next(Guid id)
    {
        var board = await _service.NextGeneration(id);
        if (board == null) return NotFound();
        return Ok(new BoardModel(_matrixMapper.To(board.State)));
    }

    [HttpGet("{id}/advance/{steps:int}")]
    public async Task<IActionResult> Steps(Guid id, int steps)
    {
        var board = await _service.Advance(id, steps);
        if (board == null) return NotFound();
        return Ok(new BoardModel(_matrixMapper.To(board.State)));
    }

    [HttpGet("{id}/final")]
    public async Task<IActionResult> Final(Guid id, int? max)
    {
        var (board, stable) = await _service.FindFinalState(id, max ?? 1000);
        if (board == null) return NotFound();

        if (!stable) return BadRequest("Board did not reach a stable state");
        return Ok(new BoardModel(_matrixMapper.To(board.State)));
    }
}