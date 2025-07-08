using BankSystem.Application.DTOs;

namespace BankSystem.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetUserDashboardAsync(Guid userId);
    }
}
