using GameOfLife.Presentation.Api.Models;
using GameOfLife.Common.Mapper;
using GameOfLife.Domain;
using GameOfLife.Applications;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GameOfLife.Presentation.Api.Controllers;

/// <summary>
/// Controller responsible for managing Game of Life boards.
/// Exposes endpoints to create boards and simulate their evolution.
/// </summary>
[ApiController]
[Route("[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IBoardApplication _service;
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;
    private readonly IMapper<Board, BoardModel> _boardMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoardsController"/>.
    /// </summary>
    /// <param name="service">Service containing the game logic.</param>
    /// <param name="matrixMapper">Mapper for converting between matrix representations.</param>
    /// <param name="boardMapper">Mapper for converting domain Board to API model.</param>
    public BoardsController(
        IBoardApplication service,
        IMapper<bool[,], List<List<bool>>> matrixMapper,
        IMapper<Board, BoardModel> boardMapper)
    {
        _service = service;
        _matrixMapper = matrixMapper;
        _boardMapper = boardMapper;
    }

    /// <summary>
    /// Creates a new board with an initial state.
    /// </summary>
    /// <param name="model">The request containing the initial matrix state.</param>
    /// <returns>The created board with its identifier and current state.</returns>
    /// <response code="200">Returns the created board</response>
    /// <response code="400">If the board size is invalid</response>
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

    /// <summary>
    /// Calculates the next generation of the board.
    /// </summary>
    /// <param name="id">The ID of the board.</param>
    /// <returns>The updated board after one generation.</returns>
    /// <response code="200">Returns the board in its next state</response>
    /// <response code="404">If the board was not found</response>
    [HttpGet("{id}/next")]
    [ProducesResponseType<BoardModel>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Next(Guid id)
    {
        var board = await _service.NextGeneration(id);
        if (board == null) return NotFound();
        return Ok(_boardMapper.To(board));
    }

    /// <summary>
    /// Advances the board by a given number of generations.
    /// </summary>
    /// <param name="id">The ID of the board.</param>
    /// <param name="steps">Number of generations to advance.</param>
    /// <returns>The board after the specified number of steps.</returns>
    /// <response code="200">Returns the updated board</response>
    /// <response code="404">If the board was not found</response>
    [HttpGet("{id}/advance/{steps:int}")]
    [ProducesResponseType<BoardModel>((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Steps(Guid id, int steps)
    {
        var board = await _service.Advance(id, steps);
        if (board == null) return NotFound();
        return Ok(_boardMapper.To(board));
    }

    /// <summary>
    /// Finds the final stable state of the board.
    /// </summary>
    /// <param name="id">The ID of the board.</param>
    /// <param name="max">Maximum number of attempts to reach stability (default: 1000).</param>
    /// <returns>The final board state and stability status.</returns>
    /// <response code="200">Returns the final stable board</response>
    /// <response code="400">If the board did not stabilize within the limit</response>
    /// <response code="404">If the board was not found</response>
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
