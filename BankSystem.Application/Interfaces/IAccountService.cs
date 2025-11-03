using BankSystem.Domain.Entities;
using BankSystem.Application.DTOs;

namespace BankSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task <Account> CreateAccountAsync(AccountDto dto);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<string> GetAccountNumberByUserIdAsync(Guid id);
        Task<bool> AccountNumberExistAsync(string accountNumber);
        Task<bool> UpdateAccountAsync(Guid id, UpdateAccountDto dto);
        Task<bool> DeleteAccountAsync(Guid id);
        Task<decimal> GetAccountBalanceAsync(Guid userId);
    }
}
