using BankSystem.Application.DTOs.Admin;
using BankSystem.Domain.Entities;

namespace BankSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetAdminDashboardAsync();
        Task<int> GetTotalUsersAsync();
        Task<object> GetUserTransactionStatsAsync();
        Task DeactivateUserAsync(Guid userId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    }
}
