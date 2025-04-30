using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IISAuthen.Models;
using IISAuthen.Models.Auth;
using IISAuthen.Models.UserRegister;
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
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("userId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> RegisterAsync(UserRegisterDto dto)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(dto.Username);
            if (existingUser != null) return false;

            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            await _userRepository.AddUserAsync(user);
            return true;
        }

        public UserProfile UserProfile()
        {
            var right = _userRepository.UserInfo.permissionInfo!.Rights!.Where(r => !string.IsNullOrEmpty(r.TaskId)).Select(r => r.TaskId).ToArray();
            var userType = _userRepository.UserInfo.permissionInfo.Role!.UserType;
            return new UserProfile()
            {
                CompName = _userRepository.CompAbbrName,
                FullName = _userRepository.FullName,
                Right = right,
                //  Right = new string[]{"UOP0000"},
                UserType = "S"
                //  UserType = ""
            };
        }
    }
}