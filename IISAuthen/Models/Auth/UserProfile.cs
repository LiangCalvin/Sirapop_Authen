namespace IISAuthen.Models.Auth
{
    public class UserProfile
    {
        public string? FullName { get; set; }
        public string? CompName { get; set; }
        public string[]? Right { get; set; }
        public string? UserType { get; set; }
    }
}