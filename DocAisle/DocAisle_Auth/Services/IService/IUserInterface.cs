using DocAisle_Auth.Db;
using DocAisle_Auth.Model;
using Microsoft.AspNetCore.Mvc;

namespace DocAisle_Auth.Services.IService
{
    public interface IUserInterface
    {

        Task<(bool IsSuccess, RegistrationResponse Response, string ErrorMessage )> GetUserById( string id);
        Task<(bool IsSuccess, LoginResponse Response, string ErrorMessage)> LoginUser(Login login);
        Task<(bool IsSuccess, bool, string ErrorMessage)> RegisterUser(RegistrationResponse registrationResponse, string userType);

    }
    
}
