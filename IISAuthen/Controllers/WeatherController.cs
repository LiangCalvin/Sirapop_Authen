using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IISAuthen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        [HttpGet("secure-data")]
        [Authorize]
        public IActionResult GetSecureData()
        {
            var username = User.Identity?.Name;
            return Ok($"Hello {username}, this is protected data.");
        }

        [HttpGet("public-data")]
        public IActionResult GetPublicData()
        {
            return Ok("This is public data. No authentication required.");
        }
    }
}