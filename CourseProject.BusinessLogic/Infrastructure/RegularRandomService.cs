using System;
using System.Collections.Generic;
using System.Text;

namespace CourseProject.BusinessLogic.Infrastructure
{
    public class RegularRandomService : IRandomService
    {
        Random random = new Random();
        public int Next(int maxValue)
        {
            return random.Next(maxValue);
        }
    }
}
