using IISAuthen.Models;
using IISAuthen.Services;
using IISAuthen.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IISAuthen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenService _authService;
        public AuthController(IAuthenService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var token = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}