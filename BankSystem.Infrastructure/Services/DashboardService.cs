using BankSystem.Application.DTOs;
using BankSystem.Application.DTOs.Admin;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly BankSystemDbContext _context;

        public DashboardService(BankSystemDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardDto> GetUserDashboardAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) throw new Exception("User not found");

            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();

            var accountIds = accounts.Select(a => a.Id).ToList();

            var transactions = await _context.Transactions
                .Where(t => accountIds.Contains(t.AccountId))
                .OrderByDescending(t => t.Timestamp)
                .Take(5)
                .ToListAsync();

            return new DashboardDto
            {
                Name = user.Name,
                Email = user.Email,
                TotalBalance = accounts.Sum(a => a.Balance),
                RecentTransactions = transactions.Select(t => new TransactionDto
                {
                    Amount = t.Amount,
                    Type = t.Type,
                    CreatedAt = t.Timestamp
                })
            };
        }
    }
}
