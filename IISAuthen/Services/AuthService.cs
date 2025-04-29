using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IISAuthen.Repositories.Interface;
using IISAuthen.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace IISAuthen.Services
{
    public class AuthService : IAuthenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;

        }

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            // Example: replace with DB check later
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Password != password)
                return null;

            if (username != "admin" || password != "password")
                return null;

            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var key = Encoding.UTF8.GetBytes(jwtKey);
            // Create claims specifically using ClaimTypes constants for standard claims
            var claims = new List<Claim>
            {
                // This is the important one - use ClaimTypes.Name for the Name claim
                new Claim(ClaimTypes.Name, username),
                // Add some additional claims for demonstration
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim("userId", "1"),
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "Bearer");
            var principal = new ClaimsPrincipal(identity);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}