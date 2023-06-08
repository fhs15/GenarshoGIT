using Microsoft.AspNetCore.Mvc;
using GenashoMatchmaker.Manager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GenashoMatchmaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchmakerController : ControllerBase
    {
        protected MatchmakerManager matchmakerManager = new();

        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<string> Get()
        {
            var joinCode = matchmakerManager.GetHost();
            if (string.IsNullOrWhiteSpace(joinCode)) return NoContent();
            return Ok(joinCode);
        }

        [HttpPost]
        public void Post(string joinCode)
        {
            matchmakerManager.AddJoinCode(joinCode);
        }
    }
}
