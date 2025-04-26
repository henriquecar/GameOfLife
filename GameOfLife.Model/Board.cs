using GameOfLife.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Model;

public class Board
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string StateJson { get; set; }
    [NotMapped]
    public bool[,] CurrentState
    {
        get => Serializer.To2D(StateJson);
        set => StateJson = JsonSerializer.Serialize(Serializer.ToJagged(value));
    }
    public Board() { }
    public Board(bool[,] state) => CurrentState = state;
}