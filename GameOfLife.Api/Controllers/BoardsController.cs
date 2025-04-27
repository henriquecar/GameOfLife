using GameOfLife.Api.Models;
using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GameOfLife.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BoardsController : ControllerBase
{
    private readonly GameOfLifeService _service;
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;
    private readonly IMapper<Board, BoardModel> _boardMapper;

    public BoardsController(GameOfLifeService service, IMapper<bool[,], List<List<bool>>> matrixMapper, IMapper<Board, BoardModel> boardMapper)
    {
        _service = service;
        _matrixMapper = matrixMapper;
        _boardMapper = boardMapper;
    }

    [HttpPost]
    [ProducesResponseType<BoardModel>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Post([FromBody] CreateBoardRequest model)
    {
        var initialState = _matrixMapper.From(model.InitialState);

        int rows = initialState.GetLength(0);
        int cols = initialState.GetLength(1);

        if (rows < 3 || cols < 3)
            return BadRequest("Board must be at least 3x3 in size.");

        if (rows > 100 || cols > 100)
            return BadRequest("Board must be at most 100x100 in size.");

        var board = await _service.SaveBoardAsync(initialState);
        return Ok(_boardMapper.To(board));
    }

    [HttpGet("{id}/next")]
    [ProducesResponseType<BoardModel>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Next(Guid id)
    {
        var board = await _service.NextGeneration(id);
        if (board == null) return NotFound();
        return Ok(_boardMapper.To(board));
    }

    [HttpGet("{id}/advance/{steps:int}")]
    [ProducesResponseType<BoardModel>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Steps(Guid id, int steps)
    {
        var board = await _service.Advance(id, steps);
        if (board == null) return NotFound();
        return Ok(_boardMapper.To(board));
    }

    [HttpGet("{id}/final")]
    [ProducesResponseType<FinalStateResult>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Final(Guid id, int? max)
    {
        var (board, stable) = await _service.FindFinalState(id, max ?? 1000);
        if (board == null) return NotFound();

        if (!stable) return BadRequest("Board did not reach a stable state");
        return Ok(new FinalStateResult(_boardMapper.To(board), stable));
    }
}