using Microsoft.AspNetCore.Mvc;
using AuthentGuard.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            RegistrationDate = DateTime.UtcNow
            // Set other properties as needed
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // User successfully registered
            return Ok("User registered successfully");
        }

        // Registration failed
        return BadRequest(result.Errors);
    }

    // Other user-related actions...
}
