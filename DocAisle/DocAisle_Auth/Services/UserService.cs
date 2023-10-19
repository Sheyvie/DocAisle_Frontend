
using AutoMapper;
using DocAisle_Auth.Db;
using DocAisle_Auth.Model;
using DocAisle_Auth.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DocAisle_Auth.Services
{
    public class UserService : IUserInterface
    {
        private readonly UsersDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtInterface _jwtInterface;

        public UserService(UsersDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtInterface jwtInterface)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtInterface = jwtInterface;

            _mapper = mapper;


        }



        public async Task<(bool IsSuccess, RegistrationResponse Response, string ErrorMessage)> GetUserById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                // Compare the hashed confirmation password with the stored hash
                var confirmMatch = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, user.ConfirmPassword);

                if (confirmMatch == PasswordVerificationResult.Success)
                {
                    var registrationResponse = _mapper.Map<RegistrationResponse>(user);
                    return (true, registrationResponse, null);
                }
                return (false, null, "Password does not match");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, LoginResponse Response, string ErrorMessage)> LoginUser(Login login)
        {


            try
            {
                var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == login.Username.ToLower());
                if (user == null)
                {
                    // User not found, return an error response.
                    return (false, null, "User not found");
                }
                var isValid = await _userManager.CheckPasswordAsync(user, login.Password);

                if (!isValid || user == null)
                {


                    new LoginResponse();
                    return (false, null, "Invalid password");

                }

                var roles = await _userManager.GetRolesAsync(user);

                var token = _jwtInterface.TokenGenerator(user, roles);
                var userLog = new LoginResponse()
                {
                    User = _mapper.Map<User>(user),
                    Token = token
                };
                return (true, userLog, null);




            }
            catch (Exception ex)
            {

                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool, string ErrorMessage)> RegisterUser(RegistrationResponse registrationResponse, string userType)

        {

            try
            {
                var user = _mapper.Map<ApplicationUser>(registrationResponse);

                // Assign the role based on the user type
                string roleToAssign = userType == "Doctor" ? "Doctor" :
                       userType == "Patient" ? "Patient" :
                       "Admin";


                // Check if the role exists, and create it if it doesn't
                if (!_roleManager.RoleExistsAsync(roleToAssign).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleToAssign)).GetAwaiter().GetResult();
                }

                // Create the user
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    // Assign the role to the user
                    await _userManager.AddToRoleAsync(user, roleToAssign);

                    return (true, true, null);
                }
                else
                {
                    var error = result.Errors.FirstOrDefault();
                    return (false, false, error?.Description);
                }
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }
    }
}
       
 

