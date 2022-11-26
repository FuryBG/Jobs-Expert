using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.AuthModels;
using WebApplication1.Models.DatabaseModels;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService _service;
        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromForm] UserLoginModel currUser)
        {
            try
            {
                string token = _service.Login(currUser);
                Response.Cookies.Append("Authorization", token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
                return Redirect("https://localhost:7285/app/search");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }

        [HttpGet("/logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Authorization");
            return Redirect("/");
        }

        [HttpGet("/userlocalinfo")]
        [Authorize]
        public IActionResult UserLocalInfo ()
        {
            var claims = User.Claims.ToList();
            int userId = int.Parse(claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value);

            User currUser = _service.GetUserLocalInfo(userId);

            return Ok(currUser);
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromForm] UserModel user)
        {
            try
            {
                string token = _service.Register(user);
                Response.Cookies.Append("Authorization", token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
                return Redirect("https://localhost:7285/app/search");
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
