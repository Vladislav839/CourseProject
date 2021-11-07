using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BusinessLogic.Interfaces
{
    public interface IGameService
    {
        public int[][] ComputerField { get; }
        public int[][] UserField { get; }
        public Task<int> CreateGame(int[][] field, string userName);

        public Task<int[][]> GetComputerField(int gameId);
        public Task<int[][]> GetUserField(int gameId);
    }
}
