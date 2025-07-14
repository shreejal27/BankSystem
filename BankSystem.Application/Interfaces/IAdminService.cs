using System.Transactions;

namespace BankSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task<int> GetTotalUsersAsync();
        Task DeactivateUserAsync(Guid userId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    }
}
