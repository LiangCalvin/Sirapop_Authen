using IISAuthen.Data;
using IISAuthen.Models;
using IISAuthen.Models.Authen;
using IISAuthen.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IISAuthen.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        private readonly List<User> _users = new()
        {
            new User { Username = "admin", Password = "password", UserId = 1, Role = "User" }
        };

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            // var user = _users.FirstOrDefault(u => u.Username == username);

            // return Task.FromResult(user);
            return await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }
        private string? _EmploymentEmail { get; set; }
        public string EmploymentEmail
        {
            get
            {
                return _EmploymentEmail!;
            }
        }
        private string? _Email { get; set; }
        public string Email
        {
            get
            {
                return _Email!;
            }
        }
        private DateTime _CurentTime { get; set; }
        public DateTime CurentTime
        {
            get
            {
                return _CurentTime;
            }
        }

        private UserData? _UserInfo { get; set; }
        public UserData UserInfo
        {
            get
            {
                return _UserInfo!;
            }
        }
        private string? _PersonCompIdReference { get; set; }
        public string PersonCompIdReference
        {
            get
            {
                return _PersonCompIdReference!;
            }
        }
        private string? _EmploymentCompIdReference { get; set; }
        public string EmploymentCompIdReference
        {
            get
            {
                return _EmploymentCompIdReference!;
            }
        }
        private string? _CompIdReference { get; set; }
        public string CompIdReference
        {
            get
            {
                return _CompIdReference!;
            }
        }
        private string? _CompAbbrName { get; set; }
        public string CompAbbrName
        {
            get
            {
                return _CompAbbrName!;
            }
        }
        private string? _IdNo { get; set; }
        public string IdNo
        {
            get
            {
                return _IdNo!;
            }
        }
        private string? _FullName { get; set; }
        public string FullName
        {
            get
            {
                return _FullName!;
            }
        }

        public void SetStateUser(HttpContext httpContext, IEncrypt encrypt)
        {
            var permissionInfoEncryption = httpContext.User.Claims.Where(x => x.Type == "permissionInfoEncryption").Select(c => c.Value).FirstOrDefault();
            var permissionInfo = httpContext.User.Claims.Where(x => x.Type == "permissionInfo").Select(c => c.Value).FirstOrDefault();
            var employmentInfo = httpContext.User.Claims.Where(x => x.Type == "employmentInfo").Select(c => c.Value).FirstOrDefault();
            var sub = httpContext.User.Claims.Where(x => x.Type == "sub").Select(c => c.Value).FirstOrDefault();

            var userInfo = new UserData
            {
                personInfo = string.IsNullOrEmpty(permissionInfoEncryption) ? null : encrypt.DecryptAES<UserDataPersonInfo>(permissionInfoEncryption),
                employmentInfo = string.IsNullOrEmpty(employmentInfo) ? null : JsonConvert.DeserializeObject<UserDataEmploymentInfo>(employmentInfo),
                permissionInfo = string.IsNullOrEmpty(permissionInfo) ? null : JsonConvert.DeserializeObject<UserDataPermissionInfo>(permissionInfo),
                sub = sub,
            };

            userInfo.currentComp = userInfo.personInfo!.CompanyOutsourceInfo ?? userInfo.personInfo.Company;

            this._Email = sub;
            this._CurentTime = DateTime.UtcNow;
            this._UserInfo = userInfo;
            this._CompIdReference = userInfo.currentComp!.CompIdReference;
            this._CompAbbrName = userInfo.currentComp.CompNameTh ?? userInfo.currentComp.CompNameEn;
            this._EmploymentEmail = userInfo.employmentInfo?.Email;
            this._PersonCompIdReference = userInfo.personInfo?.Company?.CompIdReference;
            this._EmploymentCompIdReference = userInfo.employmentInfo?.CompIdReference;
            this._IdNo = userInfo.personInfo!.idNo;
            this._FullName = userInfo.personInfo.FullNameTh;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}