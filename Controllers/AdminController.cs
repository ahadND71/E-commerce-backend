using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;
using Microsoft.AspNetCore.Identity.Data;
using api.Authentication.Service;
using api.Authentication.Dtos;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Errors.Model;

namespace api.Controllers;


[ApiController]
[Route("/api/admins")]
public class AdminController : ControllerBase
{
    private readonly AdminService _dbContext;
    private readonly AuthService _authService;
    private readonly IEmailSender _emailSender;

    public AdminController(AdminService adminService, AuthService authService, IEmailSender emailSender)
    {
        _dbContext = adminService;
        _authService = authService;
        _emailSender = emailSender;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllAdmins()
    {
        try
        {
            var admins = await _dbContext.GetAllAdminsService();
            if (admins.ToList().Count < 1)
            {
                return ApiResponse.NotFound("No Admins To Display");

            }

            return ApiResponse.Success<IEnumerable<Admin>>(
                admins,
               "Admins are returned successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Admin list");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{adminId}")]
    public async Task<IActionResult> GetAdmin(string adminId)
    {
        try
        {
            if (!Guid.TryParse(adminId, out Guid adminIdGuid))
            {
                return BadRequest("Invalid admin ID Format");
            }
            var admin = await _dbContext.GetAdminById(adminIdGuid);
            if (admin == null)
            {
                return ApiResponse.NotFound(
                 $"No Admin Found With ID : ({adminIdGuid})");
            }
            else
            {
                return ApiResponse.Success<Admin>(
                 admin,
                 "Admin is returned successfully"
               );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Admin");
            return ApiResponse.ServerError(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin newAdmin)
    {
        try
        {
            var createdAdmin = await _dbContext.CreateAdminService(newAdmin);
            if (createdAdmin != null)
            {
                return ApiResponse.Created<Admin>(createdAdmin, "Admin is created successfully");
            }
            else
            {
                return ApiResponse.ServerError("Email already exists");

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot create new Admin");
            return ApiResponse.ServerError(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginUserDto loginUserDto)
    {
        try
        {
            var LoggedAdmin = await _dbContext.LoginAdminService(loginUserDto);
            if (LoggedAdmin == null)
            {
                return ApiResponse.UnAuthorized("Invalid Credential");
            }
            var token = _authService.GenerateJwtToken(LoggedAdmin);
            return ApiResponse.Success<LoginUserDto>(LoggedAdmin, "Admin is loggedIn successfully", null, token);


        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot login");
            return ApiResponse.ServerError(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{adminId}")]
    public async Task<IActionResult> UpdateAdmin(string adminId, Admin updateAdmin)
    {
        try
        {
            if (!Guid.TryParse(adminId, out Guid adminIdGuid))
            {
                return ApiResponse.BadRequest("Invalid Admin ID Format");
            }
            var admin = await _dbContext.UpdateAdminService(adminIdGuid, updateAdmin);
            if (admin == null)
            {
                return ApiResponse.NotFound("No Admin Founded To Update");

            }
            return ApiResponse.Success<Admin>(
                admin,
                "Admin Is Updated Successfully"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot update the Admin ");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{adminId}")]
    public async Task<IActionResult> DeleteAdmin(string adminId)
    {
        try
        {
            if (!Guid.TryParse(adminId, out Guid adminIdGuid))
            {
                return ApiResponse.BadRequest("Invalid admin ID Format");
            }
            var result = await _dbContext.DeleteAdminService(adminIdGuid);
            if (!result)

            {
                return ApiResponse.NotFound("The Admin is not found to be deleted");

            }
            //new SuccessMessage<Admin>
            return ApiResponse.Success(" Admin is deleted successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, the Admin can not deleted");
            return ApiResponse.ServerError(ex.Message);

        }
    }

    //uncomment this to test it in your email but please use any domain except gmail i think you can also use temp email (preferred)
    // [AllowAnonymous]
    // [HttpPost("test")]
    // public async Task<IActionResult> Index()
    // {
    //     await _emailSender.SendEmailAsync("jixaba1294@facais.com", "hello", "how are you");
    //     return Ok();
    // }
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            var result = await _dbContext.ForgotPasswordService(email);
            if (!result)
            {
                return ApiResponse.NotFound("No user found with this email");
            }
            return ApiResponse.Success("Password reset email sent successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot send password reset link: {ex.Message}");
            return ApiResponse.ServerError(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            {
                return ApiResponse.BadRequest("Passwords do not match");
            }

            var result = await _dbContext.ResetPasswordService(resetPasswordDto);

            if (!result)
            {
                return ApiResponse.BadRequest("User with this email not found");

            }
            return ApiResponse.Success("Password reset successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot reset password: {ex.Message}");
            return ApiResponse.ServerError(ex.Message);
        }
    }

}


