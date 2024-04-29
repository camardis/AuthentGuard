using AuthentGuard.API.Models;
using AuthentGuard.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthentGuard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] LogIn logIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = _authService.Authenticate(logIn.Email, logIn.Password);

            if (token == null)
            {
                _logger.LogWarning($"Failed login attempt for user: {logIn.Email}");
                _logger.LogTrace($"Failed login attempt for user: {logIn.Email}");
                return Unauthorized();
            }

            return Ok(new TokenResponse { Token = token });
        }

        [HttpPost("check-auth")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Authenticat([FromBody] TokenResponse token)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid token validation request");
                return BadRequest(ModelState);
            }
            if (_authService.ValdidateToken(token.Token))
            {
                _logger.LogInformation($"Token validation successful");
                return Ok();
            }
            else
            {
                _logger.LogWarning($"Token validation failed");
                return Unauthorized();
            }
        }
    }
}
