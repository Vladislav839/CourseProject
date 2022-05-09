using CourseProject.BusinessLogic.Interfaces;
using CourseProject.BusinessLogic.Services;
using CourseProject.Data.Models;
using CourseProject.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IUsersService _userService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameService _gameService;

        public GameController(IServiceProvider serviceProvider, 
            IUsersService userService, IGameService gameService)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            _gameService = gameService;
        }
        public IActionResult Index(string mode)
        {
            ViewBag.Characters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            ViewBag.Avatar = _userService.GetUserByUserName(User.Identity.Name).AvatarUrl;
            ViewBag.Mode = mode;
            return View();
        }

        public async Task<IActionResult> ResumeGame()
        {
            User user = _userService.GetUserByUserName(User.Identity.Name);
            ViewBag.Avatar = _userService.GetUserByUserName(User.Identity.Name).AvatarUrl;
            List<Game> userGames = await _gameService.GetUserGames(user.Id);
            return View(userGames.Select(g => new ResumeGameViewModel { Id = g.Id, GameDateTime = g.GameDate }));
        }

        [HttpPost]
        public async Task<int> DeleteGame(int gameId)
        {
            await _gameService.DeleteGame(gameId);
            return gameId;
        }

        [HttpPost]
        public async Task EndGame(int gameId, string winner)
        {
            Game game = await _gameService.GetGameById(gameId);

            User user = _userService.GetUserById(game.UserId);
            user.TotalGames++;

            if (winner == "User")
            {
                user.Won++;
            }
            else
            {
                user.Lose++;
            }

            await _userService.UpdateUser(user);
            await _gameService.DeleteGame(gameId);
        }

        [HttpPost]
        public async Task<int> InititalizeGame([FromBody] PostData postData)
        {
            var gameService = ResolveGameServiceDependency(postData.mode);
            int id = await gameService.CreateGame(postData.field, postData.mode, User.Identity.Name);
            return id;
        }

        [HttpGet]
        public async Task<IActionResult> GameSession(int id)
        {
            ViewBag.Characters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            ViewBag.Avatar = _userService.GetUserByUserName(User.Identity.Name).AvatarUrl;
            GameViewModel viewModel = new GameViewModel
            {
                UserField = await _gameService.GetUserField(id)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MakeShot([FromBody] UserShotViewModel shot)
        {
            var game = await _gameService.GetGameById(shot.GameId);
            var gameService = ResolveGameServiceDependency(game.ComputerMode);
            CellType cellType = await gameService.MakeUsersShot(shot.Col, shot.Row, shot.GameId, CellOwner.COMPUTER);
            var gameResult = await gameService.CheckWinner(shot.GameId);
            if(gameResult.Item1 == true)
            {
                return Json(new ResponseShotViewModel { Winner = gameResult.Item2 });
            }
            var computerShot = await gameService.MakeComputerShot(shot.GameId);
            gameResult = await gameService.CheckWinner(shot.GameId);
            ResponseShotViewModel shotViewModel = new ResponseShotViewModel
            {
                Col = computerShot.Item1,
                Row = computerShot.Item2,
                GameId = shot.GameId,
                UserShotCellType = cellType.ToString(),
                ComputerShotCellType =  computerShot.Item3.ToString(),
                Winner = gameResult.Item2
            };
            return Json(shotViewModel);
        }

        private IGameService ResolveGameServiceDependency(string gameMode)
        {
            if (gameMode == "easy")
            {
                return (IGameService)_serviceProvider.GetService(typeof(GameService));
            }
            else
            {
                return (IGameService)_serviceProvider.GetService(typeof(StrategyGameService));
            }
        }

        public class PostData
        {
            public int[][] field { get; set; }
            public string mode { get; set;  }
        }
    }
}
