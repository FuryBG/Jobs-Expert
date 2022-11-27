using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Models.AuthModels;
using WebApplication1.Models.DatabaseModels;
using WebApplication1.Services;
//using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthService _authService;

        public HomeController(ILogger<HomeController> logger, AuthService service)
        {
            _authService = service;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Redirect("/app/search");
            }else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult LoginVerify([FromForm] UserLoginModel currUser)
        {
            try
            {
                string token = _authService.Login(currUser);
                Response.Cookies.Append("Authorization", token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
                return Redirect("https://localhost:7285/app/search");
            }
            catch (Exception err)
            {
                return PartialView("Login", err.Message);
            }
        }
        public IActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult VerifyRegister([FromForm] UserModel user)
        {
            try
            {
                string token = _authService.Register(user);
                Response.Cookies.Append("Authorization", token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
                return Redirect("https://localhost:7285/app/search");
            }
            catch (Exception err)
            {
                return PartialView("Register", err.Message);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}