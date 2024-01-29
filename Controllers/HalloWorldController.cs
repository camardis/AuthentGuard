using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthentGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalloWorldController : ControllerBase
    {
        // GET: api/<HalloWorldController>

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Hallo World", "From AuthentGuardAPI" };
        }

        // GET api/<HalloWorldController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HalloWorldController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HalloWorldController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HalloWorldController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
