using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Authentication.Service;
using api.Authentication.Dtos;

namespace api.Controllers;


[ApiController]
[Route("/api/admins")]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;
    private readonly AuthService _authService;
    private readonly IEmailSender _emailSender;
    public AdminController(AdminService adminService, AuthService authService, IEmailSender emailSender)
    {
        _adminService = adminService;
        _authService = authService;
        _emailSender = emailSender;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllAdmins([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        var admins = await _adminService.GetAllAdminsService(currentPage, pageSize);
        if (admins.TotalCount < 1)
        {
            return ApiResponse.NotFound("No Admins To Display");

        }

        return Ok(admins);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{adminId}")]
    public async Task<IActionResult> GetAdmin(string adminId)
    {
        if (!Guid.TryParse(adminId, out Guid adminIdGuid))
        {
            return ApiResponse.BadRequest("Invalid admin ID Format");
        }

        var admin = await _adminService.GetAdminById(adminIdGuid);
        if (admin == null)
        {
            return ApiResponse.NotFound(
             $"No Admin Found With ID : ({adminIdGuid})");
        }
        else
        {
            return ApiResponse.Success(admin,
           "Admin is returned successfully");
        }
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin newAdmin)
    {
        var createdAdmin = await _adminService.CreateAdminService(newAdmin);
        if (createdAdmin != null)
        {
            return ApiResponse.Created(createdAdmin, "Admin is created successfully");
        }
        else
        {
            return ApiResponse.ServerError("Email already exists");
        }
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginUserDto loginUserDto)
    {
        var LoggedAdmin = await _adminService.LoginAdminService(loginUserDto);
        if (LoggedAdmin == null)
        {
            return ApiResponse.UnAuthorized("Invalid Credential");
        }

        var token = _authService.GenerateJwtToken(LoggedAdmin);
        return ApiResponse.Success(LoggedAdmin, "Admin is loggedIn successfully", null, token);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{adminId}")]
    public async Task<IActionResult> UpdateAdmin(string adminId, Admin updateAdmin)
    {
        if (!Guid.TryParse(adminId, out Guid adminIdGuid))
        {
            return ApiResponse.BadRequest("Invalid Admin ID Format");
        }

        var admin = await _adminService.UpdateAdminService(adminIdGuid, updateAdmin);
        if (admin == null)
        {
            return ApiResponse.NotFound("No Admin Founded To Update");

        }
        return ApiResponse.Success<Admin>(
            admin,
            "Admin Is Updated Successfully"
        );
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{adminId}")]
    public async Task<IActionResult> DeleteAdmin(string adminId)
    {
        if (!Guid.TryParse(adminId, out Guid adminIdGuid))
        {
            return ApiResponse.BadRequest("Invalid admin ID Format");
        }

        var result = await _adminService.DeleteAdminService(adminIdGuid);
        if (!result)

        {
            return ApiResponse.NotFound("The Admin is not found to be deleted");
        }
        //new SuccessMessage<Admin>
        return ApiResponse.Success(" Admin is deleted successfully");
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
        var result = await _adminService.ForgotPasswordService(email);
        if (!result)
        {
            return ApiResponse.NotFound("No user found with this email");
        }
        return ApiResponse.Success("Password reset email sent successfully");
    }


    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
        {
            return ApiResponse.BadRequest("Passwords do not match");
        }

        var result = await _adminService.ResetPasswordService(resetPasswordDto);

        if (!result)
        {
            return ApiResponse.BadRequest("User with this email not found");
        }
        return ApiResponse.Success("Password reset successfully");
    }
}


