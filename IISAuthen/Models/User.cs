namespace IISAuthen.Models
{
    public class User
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int UserId { get; set; }
        public string Role { get; set; } = "User";
    }
}