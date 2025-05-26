using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Application.Interfaces
{
    public interface IAccountService
    {
        Task <Account> CreateAccountAsync(CreateAccountDto dto);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
    }
}
