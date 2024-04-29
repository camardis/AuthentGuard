using AuthentGuard.API.Models;
using AuthentGuard.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthentGuard.API.Controllers
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
        public IActionResult SignUp([FromBody] Register newRegister)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid registration request");
                return BadRequest(ModelState);
            }

            var result = _authService.Register(newRegister);

            if (!result.Success)
            {
                _logger.LogError($"Failed to register user: {result.Message}");
                return BadRequest(new { Error = result.Message });
            }

            var token = _authService.Authenticate(newRegister.Email, newRegister.Password);
            _logger.LogInformation($"User {newRegister.Email} registered successfully");
            return Ok(new TokenResponse { Token = token });

        }
    }
}
