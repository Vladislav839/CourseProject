using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProject.Data.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        List<MarkedCell> cells = new List<MarkedCell>();
    }
}
