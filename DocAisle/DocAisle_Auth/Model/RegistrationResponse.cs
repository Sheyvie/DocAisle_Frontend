using System.ComponentModel.DataAnnotations;

namespace DocAisle_Auth.Model
{
    public class RegistrationResponse
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } =string.Empty;
        [Required]
        public int PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword { get; set; }=string.Empty;

        public string? Role { get; set; } = string.Empty;
    }
}
