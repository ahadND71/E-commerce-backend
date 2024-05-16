using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Backend.Data;
using Backend.Dtos;
using Backend.EmailSetup;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using SendGrid.Helpers.Errors.Model;

namespace Backend.Controllers;

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
        int totalCount = await _adminService.GetTotalAdminCount();

        if (totalCount < 1)
        {
            throw new NotFoundException("No Admins To Display");
        }

        return ApiResponse.Success(admins, "Admins are returned successfully", new PaginationMeta
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{adminId:guid}")]
    public async Task<IActionResult> GetAdmin(Guid adminId)
    {
        var admin = await _adminService.GetAdminById(adminId) ?? throw new NotFoundException($"No Admin Found With ID : ({adminId})");

        return ApiResponse.Success(admin, "Admin is returned successfully");
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin newAdmin)
    {
        var createdAdmin = await _adminService.CreateAdminService(newAdmin) ?? throw new Exception("Email already exists");

        return ApiResponse.Created(createdAdmin, "Admin is created successfully");
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginUserDto loginUserDto)
    {
        var LoggedAdmin = await _adminService.LoginAdminService(loginUserDto) ?? throw new UnauthorizedException("Invalid Email or Password");

        var token = _authService.GenerateJwtToken(LoggedAdmin);
        return ApiResponse.Success(LoggedAdmin, "Admin is loggedIn successfully", null, token);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{adminId}")]
    public async Task<IActionResult> UpdateAdmin(string adminId, AdminDto updateAdmin)
    {
        if (!Guid.TryParse(adminId, out Guid adminIdGuid))
        {
            throw new ValidationException("Invalid Admin ID Format");
        }

        var admin = await _adminService.UpdateAdminService(adminIdGuid, updateAdmin) ?? throw new NotFoundException("No Admin Founded To Update");

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
            throw new ValidationException("Invalid admin ID Format");
        }

        var result = await _adminService.DeleteAdminService(adminIdGuid);
        if (!result)

        {
            throw new NotFoundException("The Admin is not found to be deleted");
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
            throw new NotFoundException("No user found with this email");
        }

        return ApiResponse.Success("Password reset email sent successfully");
    }


    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
        {
            throw new ValidationException("Passwords do not match");
        }

        var result = await _adminService.ResetPasswordService(resetPasswordDto);

        if (!result)
        {
            throw new NotFoundException("User with this email not found");
        }

        return ApiResponse.Success("Password reset successfully");
    }
}