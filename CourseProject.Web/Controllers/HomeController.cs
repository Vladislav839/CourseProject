using CourseProject.BusinessLogic.Interfaces;
using CourseProject.Data.Models;
using CourseProject.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(IUsersService usersService, IWebHostEnvironment appEnvironment)
        {
            _usersService = usersService;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            User user = await Task.Run(() => _usersService.GetUserByUserName(User.Identity.Name));
            UserViewModel userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                AvatarUrl = user.AvatarUrl != null ? user.AvatarUrl : "/img/avatar_default.png",
                Email = user.Email,
                TotalGames = user.TotalGames,
                Lose = user.Lose,
                Won = user.Won
            };
            ViewBag.Avatar = userViewModel.AvatarUrl;
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = Path.Combine("img", uploadedFile.FileName);
                using (var fileStream = new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                User user = _usersService.GetUserByUserName(User.Identity.Name);
                user.AvatarUrl = "\\" + path;
                await _usersService.UpdateUser(user);
            }

            return RedirectToAction("Profile");
        }
    }
}
