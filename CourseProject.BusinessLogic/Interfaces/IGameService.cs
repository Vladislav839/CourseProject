using CourseProject.Data.Models;
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

        public Task<CellType> MakeUsersShot(int col, int row, int gameId, CellOwner owner);
        public Task<(int, int, CellType)> MakeComputerShot(int gameId);

        public Task<(bool, string)> CheckWinner(int gameId);

        public Task<Game> GetGameById(int gameId);

        public Task DeleteGame(int id);
        public Task<List<Game>> GetUserGames(string userId);
    }
}
