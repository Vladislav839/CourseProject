using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Не указан Email")]
        public string Password { get; set; }
    }
}
