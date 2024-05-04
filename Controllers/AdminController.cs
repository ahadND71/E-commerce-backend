using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;
using api.Authentication.Identity;

namespace api.Controllers;


[ApiController]
[Route("/api/admins")]
public class AdminController : ControllerBase
{
    private readonly AdminService _dbContext;
    public AdminController(AdminService adminService)
    {
        _dbContext = adminService;
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAdmins()
    {
        try
        {
            var admins = await _dbContext.GetAllAdminsService();
            if (admins.ToList().Count < 1)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Admins To Display"
                });
            }

            return Ok(new SuccessMessage<IEnumerable<Admin>>
            {
                Message = "Admins are returned successfully",
                Data = admins
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Admin list");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [AllowAnonymous]
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
                return NotFound(new ErrorMessage
                {
                    Message = $"No Admin Found With ID : ({adminIdGuid})"
                });
            }
            else
            {
                return Ok(new SuccessMessage<Admin>
                {
                    Success = true,
                    Message = "Admin is returned successfully",
                    Data = admin
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Admin");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin newAdmin)
    {
        try
        {
            var createdAdmin = await _dbContext.CreateAdminService(newAdmin);
            if (createdAdmin != null)
            {
                return CreatedAtAction(nameof(GetAdmin), new { adminId = createdAdmin.AdminId }, createdAdmin);
            }
            return Ok(new SuccessMessage<Admin>
            {
                Message = "Admin is created successfully",
                Data = createdAdmin
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot create new Admin");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpPut("{adminId}")]
    public async Task<IActionResult> UpdateAdmin(string adminId, Admin updateAdmin)
    {
        try
        {
            if (!Guid.TryParse(adminId, out Guid adminIdGuid))
            {
                return BadRequest("Invalid admin ID Format");
            }
            var admin = await _dbContext.UpdateAdminService(adminIdGuid, updateAdmin);
            if (admin == null)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Admin To Founded To Update"
                });
            }
            return Ok(new SuccessMessage<Admin>
            {
                Message = "Admin Is Updated Successfully",
                Data = admin
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot update the Admin ");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpDelete("{adminId}")]
    public async Task<IActionResult> DeleteAdmin(string adminId)
    {
        try
        {
            if (!Guid.TryParse(adminId, out Guid adminIdGuid))
            {
                return BadRequest("Invalid admin ID Format");
            }
            var result = await _dbContext.DeleteAdminService(adminIdGuid);
            if (!result)

            {
                return NotFound(new ErrorMessage
                {
                    Message = "The Admin is not found to be deleted"
                });
            }
            //new SuccessMessage<Admin>
            return Ok(new { success = true, message = " Admin is deleted successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, the Admin can not deleted");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }
}
