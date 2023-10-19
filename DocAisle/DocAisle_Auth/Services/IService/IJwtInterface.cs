using DocAisle_Auth.Model;

namespace DocAisle_Auth.Services.IService
{
    public interface IJwtInterface
    {
        string TokenGenerator(ApplicationUser user, IEnumerable<string> roles);
    }
}
