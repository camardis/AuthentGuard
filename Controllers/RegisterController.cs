using AuthentGuard.Models;
using AuthentGuard.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthentGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserService _userService;

        public RegisterController(UserService userService)
        {
            _userService = userService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            if (_userService.RegisterUser(model))
            {
                return Ok(new { Message = "Registration successful!" });
            }

            return BadRequest(new { Message = "Registration failed." });
        }
    }
}
