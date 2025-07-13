using BankSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var totalUsers = await _adminService.GetTotalUsersAsync();
            return Ok(new
            {
                TotalUsers = totalUsers,
            });
        }

        [HttpPut("users/{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(Guid id)
        {
            await _adminService.DeactivateUserAsync(id);
            return Ok(new { message = "User deactivated successfully" });
        }
    }
}
