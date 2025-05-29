using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BankSystem.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankSystemDbContext _context;

        public AccountService (BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Account> CreateAccountAsync(AccountDto dto)
        {
            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                Balance = 0
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }
    }
}
