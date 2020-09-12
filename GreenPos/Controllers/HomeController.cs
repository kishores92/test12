using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GreenPOS.Models;
using GreenPOS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using GreenPOS.Common;
using System;

namespace GreenPOS.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _uService;

        public HomeController(ILogger<HomeController> logger, IUserService uService)
        {
            _logger = logger;
            _uService = uService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        public IActionResult Index()
        {
            var ss = UserId;
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult SignIn(LoginViewModel vm)
        {
            var result = _uService.Authenticate(vm);
            if (result != null)
            {
                HttpContext.Session.SetString(Constants.Token.ToString(), result.Token);
                HttpContext.Session.SetString(Constants.UserId.ToString(), Convert.ToString(result.Id));
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }
    }
}
