using GameOfLife.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Repository.EF
{
    public class BoardEF : Board
    {
        public BoardEF() { }

        public BoardEF(Guid id, bool[,] currentState)
        {
            Id = id;
            State = currentState;
        }

        [NotMapped]
        public override bool[,] State { get; set; } = new bool[0, 0];

        public string StateJson { get; set; } = string.Empty;
    }
}
