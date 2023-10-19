namespace DocAisle_Auth.Model
{
    public class LoginResponse
    {
        public User User { get; set; } = default!;
        public string Token { get; set; } = string.Empty;
    }
}
