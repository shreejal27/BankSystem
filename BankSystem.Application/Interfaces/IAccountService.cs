using BankSystem.Domain.Entities;
using BankSystem.API.DTOs;

namespace BankSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task <Account> CreateAccountAsync(CreateAccountDto dto);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
    }
}
