using DocAisle_Auth.Model;
using DocAisle_Auth.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Security.Authentication;


namespace DocAisle_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserController(IUserInterface userInterface,UserManager<ApplicationUser> userManager)
        {
            _userInterface = userInterface;
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegAndAssignRole(RegistrationResponse registrationResponse, string userType)
        {
            var result = await _userInterface.RegisterUser(registrationResponse, userType);
            if (result.IsSuccess)
            {
                return Ok(result.IsSuccess);

            }
           

            return BadRequest("Unable to assign role,try again");
        }

        [HttpGet("verify-password/{id}")]
        public async Task<IActionResult> VerifyPassword(string id)
        {
            var result = await _userInterface.GetUserById(id);
            if (result.IsSuccess)
            {
                // Password is verified
                return Ok("Password is verified");
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(Login login)
        {
            var result = await _userInterface.LoginUser(login);
            if (result.IsSuccess)
            {
                return Ok(result.Response);

            }
            return BadRequest("Invalid Credentials");
        }
      


        [HttpPost("approve-doctor/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveDoctor(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Check if the user is already approved
            if (user.IsApproved)
            {
                return BadRequest("User is already approved");
            }

            // Set the user as approved
            user.IsApproved = true;
            await _userManager.UpdateAsync(user);

            // Generate a password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Send the password reset token to the doctor via email (implement this logic)

            return Ok("User approved");
        }

        [HttpDelete("delete-doctor/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Remove the "Doctor" role from the user
            var result = await _userManager.RemoveFromRoleAsync(user, "Doctor");
            if (!result.Succeeded)
            {
                return BadRequest("Failed to remove the Doctor role");
            }

            // You can choose to delete the user's record from the database if needed

            return Ok("Doctor removed");
        }
    }

    

}
