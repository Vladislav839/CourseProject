using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProject.Data.Models
{
    public class MarkedCell
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public CellOwner CellOwner { get; set; }
        public CellType CellType { get; set; }

    }

    public enum CellType
    {
        EMPTY,
        SHIP,
        MISS,
        HIT
    }

    public enum CellOwner
    {
        USER,
        COMPUTER
    }
}
