using IISAuthen.Models;

namespace IISAuthen.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);

    }
}