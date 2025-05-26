using BankSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankSystemDbContext _context;

        public AccountService (BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccountAsync(CreateAccountDto dto)
        {
            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                HolderName = dto.HolderName,
                Balance = 0
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }
    }
}
