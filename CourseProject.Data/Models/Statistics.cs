using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CourseProject.Data.Models
{
    public class Statistics
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public int User { get; set; }
        public int TotalGames { get; set; }
        public int Won { get; set; }
        public int Lose { get; set; }
    }
}
