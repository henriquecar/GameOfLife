using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GameOfLife.Model;

public class Board
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public virtual bool[,] State { get; set; } = new bool[0, 0];
    public virtual int Step { get; set; } = 0;

    public Board() { }
    public Board(bool[,] state) => State = state;
}