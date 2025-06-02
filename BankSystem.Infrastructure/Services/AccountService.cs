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

        public async Task<Account?> GetAccountByIdAsync(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<bool> UpdateAccountAsync(Guid id, UpdateAccountDto dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            account.Balance = dto.Balance;
            account.AccountNumber = dto.AccountNumber;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
