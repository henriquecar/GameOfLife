using GameOfLife.Presentation.Api.Models;
using GameOfLife.Common.Mapper;
using GameOfLife.Domain;
using GameOfLife.Persistence.EF;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Presentation.Api.Mapper;

/// <summary>
/// Maps between the domain-level <see cref="Board"/> entity and the API-facing <see cref="BoardModel"/>.
/// Handles conversion of matrix formats through an injected matrix mapper.
/// </summary>
public class BoardMapper : IMapper<Board, BoardModel>
{
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoardMapper"/> class.
    /// </summary>
    /// <param name="matrixMapper">
    /// A mapper responsible for converting between multidimensional (bool[,]) and jagged (List&lt;List&lt;bool&gt;&gt;) matrices.
    /// </param>
    public BoardMapper(IMapper<bool[,], List<List<bool>>> matrixMapper)
    {
        _matrixMapper = matrixMapper;
    }

    /// <summary>
    /// Converts a <see cref="BoardModel"/> to a <see cref="Board"/> domain entity.
    /// </summary>
    /// <param name="entity">The incoming model to convert.</param>
    /// <returns>A domain-level board entity.</returns>
    public Board From(BoardModel entity) =>
        new Board
        {
            Id = entity.Id,
            State = _matrixMapper.From(entity.State)
        };

    /// <summary>
    /// Converts a <see cref="Board"/> domain entity to a <see cref="BoardModel"/> suitable for API responses.
    /// </summary>
    /// <param name="entity">The domain board to convert.</param>
    /// <returns>An API-facing board model.</returns>
    public BoardModel To(Board entity) =>
        new BoardModel(entity.Id, _matrixMapper.To(entity.State), entity.Step);
}
