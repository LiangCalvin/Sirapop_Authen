
using IISAuthen.Models.Auth;
using IISAuthen.Models.UserRegister;

namespace IISAuthen.Services.Interface
{
    public interface IAuthenService
    {
        Task<string?> AuthenticateAsync(string username, string password);
        UserProfile UserProfile();
        Task<bool> RegisterAsync(UserRegisterDto dto);
    }
}