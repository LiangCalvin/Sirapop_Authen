using IISAuthen.Models;
using IISAuthen.Models.Authen;

namespace IISAuthen.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user); string IdNo { get; }
        string FullName { get; }
        string Email { get; }
        string EmploymentEmail { get; }
        string CompIdReference { get; }
        string PersonCompIdReference { get; }
        string EmploymentCompIdReference { get; }
        string CompAbbrName { get; }
        DateTime CurentTime { get; }
        UserData UserInfo { get; }
        void SetStateUser(HttpContext httpContext, IEncrypt encrypt);
    }
}