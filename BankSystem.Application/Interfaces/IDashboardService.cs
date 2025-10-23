using BankSystem.Application.DTOs;
using BankSystem.Application.DTOs.Admin;

namespace BankSystem.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetUserDashboardAsync(Guid userId);
        Task<AdminDashboardDto> GetAdminDashboardAsync();
    }
}
