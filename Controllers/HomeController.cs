using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Models.AuthModels;
//using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly AuthService _authService;

        public HomeController(ILogger<HomeController> logger /*AuthService service*/)
        {
            //_authService = service;
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

        public IActionResult Register()
        {
            return PartialView();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}