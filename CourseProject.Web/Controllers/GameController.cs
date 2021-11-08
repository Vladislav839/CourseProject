using CourseProject.BusinessLogic.Interfaces;
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
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
        public IActionResult Index()
        {
            ViewBag.Characters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            return View();
        }

        [HttpPost]
        public async Task<int> InititalizeGame([FromBody] int[][] field)
        {
            int id = await _gameService.CreateGame(field, User.Identity.Name);
            return id;
        }

        [HttpGet]
        public async Task<IActionResult> GameSession(int id)
        {
            ViewBag.Characters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            GameViewModel viewModel = new GameViewModel
            {
                UserField = await _gameService.GetUserField(id)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MakeShot([FromBody] UserShotViewModel shot)
        {
            CellType cellType = await _gameService.MakeUsersShot(shot.Col, shot.Row, shot.GameId, CellOwner.COMPUTER);
            var gameResult = await _gameService.CheckWinner(shot.GameId);
            if(gameResult.Item1 == true)
            {
                return Json(new ResponseShotViewModel { Winner = gameResult.Item2 });
            }
            var computerShot = await _gameService.MakeComputerShot(shot.GameId);
            ResponseShotViewModel shotViewModel = new ResponseShotViewModel
            {
                Col = computerShot.Item1,
                Row = computerShot.Item2,
                GameId = shot.GameId,
                UserShotCellType = cellType.ToString(),
                ComputerShotCellType =  computerShot.Item3.ToString()
            };
            return Json(shotViewModel);
        }
    }
}
