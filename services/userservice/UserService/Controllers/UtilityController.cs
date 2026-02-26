using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        [HttpGet("machinename")]
        public IActionResult Get()
        {
            return Ok(Environment.MachineName);
        }
    }
}
