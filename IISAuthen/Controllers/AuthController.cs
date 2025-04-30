using IISAuthen.Models;
using IISAuthen.Models.UserRegister;
using IISAuthen.Services;
using IISAuthen.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IISAuthen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthenService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var token = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
        [HttpGet("userprofile")]
        public IActionResult UserProfile()
        {
            var result = _authService.UserProfile();
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result)
                return BadRequest("Username already exists.");

            return Ok("User registered successfully.");
        }
    }
}