using CourseProject.BusinessLogic.Interfaces;
using CourseProject.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                ComputerField = await _gameService.GetComputerField(id),
                UserField = await _gameService.GetUserField(id)
            };

            return View(viewModel);
        }
    }
}
