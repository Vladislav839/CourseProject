using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Web.ViewModels
{
    public class ResponseShotViewModel : UserShotViewModel
    {
        public string UserShotCellType { get; set; }
        public string ComputerShotCellType { get; set; }
        public string Winner { get; set; }
    }
}
