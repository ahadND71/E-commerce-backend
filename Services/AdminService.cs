using Microsoft.EntityFrameworkCore;
using api.Data;
using Microsoft.AspNetCore.Identity;
using api.Authentication.Dtos;

using api.Helpers;
using AutoMapper;

namespace api.Services;


public class AdminService
{
  private readonly AppDbContext _adminDbContext;
  private readonly IPasswordHasher<Admin> _passwordHasher;
  private readonly IEmailSender _emailSender;
  private readonly IMapper _mapper;

  public AdminService(AppDbContext adminDbContext, IPasswordHasher<Admin> passwordHasher, IEmailSender emailSender, IMapper mapper)
  {
    _adminDbContext = adminDbContext;
    _passwordHasher = passwordHasher;
    _emailSender = emailSender;
    _mapper = mapper;
  }


  public async Task<PaginationResult<AdminDto>> GetAllAdminsService(int currentPage, int pageSize)
  {
    var totalAdminCount = await _adminDbContext.Admins.CountAsync();
    var admins = await _adminDbContext.Admins
    .Skip((currentPage - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    var adminDtos = _mapper.Map<List<Admin>, List<AdminDto>>(admins);
    return new PaginationResult<AdminDto>
    {
      Items = adminDtos,
      TotalCount = totalAdminCount,
      CurrentPage = currentPage,
      PageSize = pageSize,
    };
  }


  public async Task<AdminDto?> GetAdminById(Guid adminId)
  {
    var admin = await _adminDbContext.Customers.FindAsync(adminId);
    var adminDto = _mapper.Map<AdminDto>(admin);
    return adminDto;
  }


  public async Task<Admin?> CreateAdminService(Admin newAdmin)
  {
    bool userExist = await IsEmailExists(newAdmin.Email);
    if (userExist)
    {
      return null;
    }

    newAdmin.AdminId = Guid.NewGuid();
    newAdmin.CreatedAt = DateTime.UtcNow;
    newAdmin.Password = _passwordHasher.HashPassword(newAdmin, newAdmin.Password);
    _adminDbContext.Admins.Add(newAdmin);
    await _adminDbContext.SaveChangesAsync();
    return newAdmin;
  }


  public async Task<LoginUserDto?> LoginAdminService(LoginUserDto loginUserDto)
  {
    var admin = await _adminDbContext.Admins.SingleOrDefaultAsync(a => a.Email == loginUserDto.Email);
    if (admin == null)
    {
      return null;
    }

    var result = _passwordHasher.VerifyHashedPassword(admin, admin.Password, loginUserDto.Password);
    loginUserDto.UserId = admin.AdminId;
    loginUserDto.IsAdmin = true;
    return result == PasswordVerificationResult.Success ? loginUserDto : null;
  }


  public async Task<Admin?> UpdateAdminService(Guid adminId, Admin updateAdmin)
  {
    var existingAdmin = await _adminDbContext.Admins.FindAsync(adminId);
    if (existingAdmin != null)
    {
      existingAdmin.FirstName = updateAdmin.FirstName ?? existingAdmin.FirstName;
      existingAdmin.LastName = updateAdmin.LastName ?? existingAdmin.LastName;
      existingAdmin.Email = updateAdmin.Email ?? existingAdmin.Email;
      existingAdmin.Password = updateAdmin.Password != null ? _passwordHasher.HashPassword(updateAdmin, updateAdmin.Password) : existingAdmin.Password;
      existingAdmin.Mobile = updateAdmin.Mobile ?? existingAdmin.Mobile;
      existingAdmin.Image = updateAdmin.Image ?? existingAdmin.Image;
      await _adminDbContext.SaveChangesAsync();
    }
    return existingAdmin;
  }


  public async Task<bool> DeleteAdminService(Guid adminId)
  {
    var adminToRemove = await _adminDbContext.Admins.FindAsync(adminId);
    if (adminToRemove != null)
    {
      _adminDbContext.Admins.Remove(adminToRemove);
      await _adminDbContext.SaveChangesAsync();
      return true;
    }

    return false;
  }


  public async Task<bool> ForgotPasswordService(string email)
  {
    var admin = await _adminDbContext.Admins.FirstOrDefaultAsync(a => a.Email == email);
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
    await _adminDbContext.SaveChangesAsync();
    return true;
  }


  public async Task<bool> ResetPasswordService(ResetPasswordDto resetPasswordDto)
  {
    var admin = await _adminDbContext.Admins.FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);
    if (admin == null || admin.ResetToken != resetPasswordDto.Token || admin.ResetTokenExpiration < DateTime.UtcNow)
    {
      return false;
    }
    admin.Password = _passwordHasher.HashPassword(admin, resetPasswordDto.NewPassword);
    admin.ResetToken = null;
    admin.ResetTokenExpiration = null;
    await _adminDbContext.SaveChangesAsync();
    return true;
  }


  public async Task<bool> IsEmailExists(string email)
  {
    return await _adminDbContext.Admins.AnyAsync(a => a.Email == email) || await _adminDbContext.Customers.AnyAsync(c => c.Email == email);
  }
}