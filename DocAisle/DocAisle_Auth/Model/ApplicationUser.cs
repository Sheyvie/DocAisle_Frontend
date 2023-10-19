using Microsoft.AspNetCore.Identity;

namespace DocAisle_Auth.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool IsApproved { get; set; }=false;
    }
}
