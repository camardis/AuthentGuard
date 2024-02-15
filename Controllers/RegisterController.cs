using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthentGuard.Models;
using AuthentGuard.Services;
using Microsoft.Extensions.Logging;

namespace AuthentGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(AuthService authService, ILogger<RegisterController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SignUp([FromBody] RegisterModel RegModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _authService.Register(RegModel);

            if (!result.Success)
            {
                _logger.LogError($"Failed to register user: {result.Message}");
                return BadRequest(new { Error = result.Message });
            }

            var token = _authService.Authenticate(RegModel.Email, RegModel.Password);
            return Ok(new TokenResponse { Token = token });

        }
    }
}
