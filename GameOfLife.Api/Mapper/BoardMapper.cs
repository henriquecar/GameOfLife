using GameOfLife.Api.Models;
using GameOfLife.Common.Mapper;
using GameOfLife.Model;
using GameOfLife.Repository.EF;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Api.Mapper;

public class BoardMapper : IMapper<Board, BoardModel>
{
    private readonly IMapper<bool[,], List<List<bool>>> _matrixMapper;

    public BoardMapper(IMapper<bool[,], List<List<bool>>> matrixMapper)
    {
        _matrixMapper = matrixMapper;
    }
    public Board From(BoardModel entity) => new Board { Id = entity.Id, State = _matrixMapper.From(entity.State) };

    public BoardModel To(Board entity) => new BoardModel(entity.Id, _matrixMapper.To(entity.State), entity.Step);
}