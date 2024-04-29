using Microsoft.AspNetCore.Mvc;
using api.Services;

namespace api.Controllers;

[ApiController]
[Route("/api/admins")]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;
    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    public IActionResult GetAllAdmins()
    {
        var admins = _adminService.GetAllAdminsService();
        return Ok(admins);
    }

    [HttpGet("{adminId}")]
    public IActionResult GetAdmin(string adminId)
    {
        if (!Guid.TryParse(adminId, out Guid adminIdGuid))
        {
            return BadRequest("Invalid admin ID Format");
        }
        var admin = _adminService.GetAdminById(adminIdGuid);
        if (admin == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(admin);
        }

    }

    [HttpPost]
    public async Task<IActionResult> CreateAdmin(Admin newAdmin)
    {
        var createdAdmin = await _adminService.CreateAdminService(newAdmin);
        return CreatedAtAction(nameof(GetAdmin), new { adminId = createdAdmin.AdminId }, createdAdmin);
    }


    [HttpPut("{adminId}")]
    public IActionResult UpdateAdmin(string adminId, Admin updateAdmin)
    {
        if (!Guid.TryParse(adminId, out Guid adminIdGuid))
        {
            return BadRequest("Invalid admin ID Format");
        }
        var admin = _adminService.UpdateAdminService(adminIdGuid, updateAdmin);
        if (admin == null)
        {
            return NotFound();
        }
        return Ok(admin);
    }


    [HttpDelete("{adminId}")]
    public async Task<IActionResult> DeleteAdmin(string adminId)
    {
        if (!Guid.TryParse(adminId, out Guid adminIdGuid))
        {
            return BadRequest("Invalid admin ID Format");
        }
        var result = await _adminService.DeleteAdminService(adminIdGuid);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

}
