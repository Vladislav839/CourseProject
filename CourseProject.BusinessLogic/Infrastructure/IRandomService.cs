using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.BusinessLogic.Infrastructure
{
    public interface IRandomService
    {
        int Next(int maxValue);
    }
}
