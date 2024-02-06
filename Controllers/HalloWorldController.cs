using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthentGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalloWorldController : ControllerBase
    {
        // GET: api/<HalloWorldController>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Hallo World", "From AuthentGuardAPI" };
        }
    }
}
