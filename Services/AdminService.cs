using Microsoft.EntityFrameworkCore;
using api.Data;
using Microsoft.AspNetCore.Identity;

namespace api.Services;

public class AdminService
{

  private readonly AppDbContext _dbContext;
  private readonly IPasswordHasher<Admin> _passwordHasher;
  public AdminService(AppDbContext dbContext, IPasswordHasher<Admin> passwordHasher)
  {
    _dbContext = dbContext;
    _passwordHasher = passwordHasher;

  }


  public async Task<IEnumerable<Admin>> GetAllAdminsService()
  {
    return await _dbContext.Admins.ToListAsync();
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

}