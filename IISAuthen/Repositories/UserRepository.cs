using IISAuthen.Models;
using IISAuthen.Repositories.Interface;

namespace IISAuthen.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { Username = "admin", Password = "password", UserId = 1, Role = "User" }
        };

        public Task<User?> GetUserByUsernameAsync(string username)
        {
            // var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            var user = _users.FirstOrDefault(u => u.Username == username);

            return Task.FromResult(user);
        }
    }
}