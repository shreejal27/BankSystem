using BankSystem.Domain.Entities;
using BankSystem.Application.DTOs;

namespace BankSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task <Account> CreateAccountAsync(AccountDto dto);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
    }
}
