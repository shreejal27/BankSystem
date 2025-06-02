using BankSystem.Domain.Entities;
using BankSystem.Application.DTOs;

namespace BankSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task <Account> CreateAccountAsync(AccountDto dto);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task<bool> UpdateAccountAsync(Guid id, UpdateAccountDto dto);
        Task<bool> DeleteAccountAsync(Guid id);
    }
}
