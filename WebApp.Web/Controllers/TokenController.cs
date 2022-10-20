using Microsoft.AspNetCore.Mvc;
using WebApp.Infrastructure.Interfaces;
using WebApp.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;

        public TokenController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/Token/Authorization
        [HttpPost(Name = "Authorization")]
        public async Task<IActionResult> Token(LoginModel loginModel)
        {
            return Ok(await _userService.GetTokenAsync(loginModel));
        }
    }
}
