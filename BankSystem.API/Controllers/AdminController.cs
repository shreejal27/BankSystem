using BankSystem.Application.Interfaces;
using BankSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> GetAdminDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var adminDashboard = await _adminService.GetAdminDashboardAsync();
            return Ok(adminDashboard);
        }

        [HttpGet("user-transaction-stats")]
        public async Task<IActionResult> GetUserTransactionStats()
        {
            var result = await _adminService.GetUserTransactionStatsAsync();
            return Ok(result);
        }

        [HttpPost("user/activate/{id}")]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            await _adminService.ActivateUserAsync(id);
            return Ok(new { message = "User activated successfully" });
        }

        [HttpPost("user/deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(Guid id)
        {
            await _adminService.DeactivateUserAsync(id);
            return Ok(new { message = "User deactivated successfully" });
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _adminService.GetAllTransactionsAsync();
            return Ok(transactions);
        }
    }
}
