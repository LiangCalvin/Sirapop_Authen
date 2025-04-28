
namespace IISAuthen.Services.Interface
{
    public interface IAuthenService
    {
        Task<string?> AuthenticateAsync(string username, string password);

    }
}