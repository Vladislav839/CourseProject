using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProject.Data.Models
{
    public class User : IdentityUser
    {
        public string AvatarUrl { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();
        public int TotalGames { get; set; }
        public int Won { get; set; }
        public int Lose { get; set; }
    }
}
