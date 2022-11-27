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

    }
}
