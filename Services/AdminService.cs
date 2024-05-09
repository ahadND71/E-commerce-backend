using Microsoft.EntityFrameworkCore;
using api.Data;
using Microsoft.AspNetCore.Identity;
using api.Authentication.Dtos;

using api.Helpers;

using SendGrid;
using SendGrid.Helpers.Mail;


namespace api.Services;

public class AdminService
{

  private readonly AppDbContext _dbContext;
  private readonly IPasswordHasher<Admin> _passwordHasher;
  private readonly IEmailSender _emailSender;

  public AdminService(AppDbContext dbContext, IPasswordHasher<Admin> passwordHasher, IEmailSender emailSender)
  {
    _dbContext = dbContext;
    _passwordHasher = passwordHasher;
    _emailSender = emailSender;

  }


  public async Task<PaginationResult<Admin>> GetAllAdminsService(int currentPage , int pageSize)
  {
    var totalAdminCount = await _dbContext.Admins.CountAsync();
    var admin = await _dbContext.Admins
    .Skip((currentPage -1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
    return new PaginationResult<Admin>{
      Items = admin,
      TotalCount = totalAdminCount,
      CurrentPage = currentPage,
      PageSize = pageSize,};

  }


  public async Task<Admin?> GetAdminById(Guid adminId)
  {
    return await _dbContext.Admins.FindAsync(adminId);
  }


  public async Task<Admin> CreateAdminService(Admin newAdmin)
  {
    newAdmin.AdminId = Guid.NewGuid();
    newAdmin.CreatedAt = DateTime.UtcNow;
    newAdmin.Password = _passwordHasher.HashPassword(newAdmin, newAdmin.Password);
    _dbContext.Admins.Add(newAdmin);
    await _dbContext.SaveChangesAsync();
    return newAdmin;
  }

  public async Task<LoginUserDto?> LoginAdminService(LoginUserDto loginUserDto)
  {
    var admin = await _dbContext.Admins.SingleOrDefaultAsync(a => a.Email == loginUserDto.Email);
    if (admin == null)
    {
      return null;
    }
    var result = _passwordHasher.VerifyHashedPassword(admin, admin.Password, loginUserDto.Password);
    loginUserDto.UserId = admin.AdminId;
    loginUserDto.IsAdmin = true;
    return result == PasswordVerificationResult.Success ? loginUserDto : null;

  }


  public async Task<Admin> UpdateAdminService(Guid adminId, Admin updateAdmin)
  {
    var existingAdmin = await _dbContext.Admins.FindAsync(adminId);
    if (existingAdmin != null)
    {
      existingAdmin.FirstName = updateAdmin.FirstName ?? existingAdmin.FirstName;
      existingAdmin.LastName = updateAdmin.LastName ?? existingAdmin.LastName;
      existingAdmin.Email = updateAdmin.Email ?? existingAdmin.Email;
      existingAdmin.Password = updateAdmin.Password != null ? _passwordHasher.HashPassword(updateAdmin, updateAdmin.Password) : existingAdmin.Password;
      existingAdmin.Mobile = updateAdmin.Mobile ?? existingAdmin.Mobile;
      existingAdmin.Image = updateAdmin.Image ?? existingAdmin.Image;
      await _dbContext.SaveChangesAsync();
    }
    return existingAdmin;
  }


  public async Task<bool> DeleteAdminService(Guid adminId)
  {
    var adminToRemove = await _dbContext.Admins.FindAsync(adminId);
    if (adminToRemove != null)
    {
      _dbContext.Admins.Remove(adminToRemove);
      await _dbContext.SaveChangesAsync();
      return true;
    }
    return false;
  }

  public async Task<bool> ForgotPasswordService(string email)
  {
    var admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Email == email);
    if (admin == null)
    {
      return false;
    }

    var resetToken = Guid.NewGuid();

    admin.ResetToken = resetToken;
    admin.ResetTokenExpiration = DateTime.UtcNow.AddHours(1);
    // bc we still not have real host so i will just send a token so we can test it using swagger in the production adjust this 2 lines
    // string resetLink = $"http://localhost:5125/api/admins/reset-password?email={email}&token={resetToken}";

    await _emailSender.SendEmailAsync(email, "Password Reset", $"Dear {admin.FirstName},\nThis is your token {resetToken} to reset your password");
    await _dbContext.SaveChangesAsync();
    return true;

  }

  public async Task<bool> ResetPasswordService(ResetPasswordDto resetPasswordDto)
  {
    var admin = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
    if (admin == null || admin.ResetToken != resetPasswordDto.Token || admin.ResetTokenExpiration < DateTime.UtcNow)
    {
      return false;
    }
    admin.Password = _passwordHasher.HashPassword(admin, resetPasswordDto.NewPassword);
    admin.ResetToken = null;
    admin.ResetTokenExpiration = null;
    await _dbContext.SaveChangesAsync();
    return true;

  }




}