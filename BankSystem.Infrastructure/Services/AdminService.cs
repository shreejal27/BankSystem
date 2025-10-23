using BankSystem.Application.DTOs.Admin;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly BankSystemDbContext _context;

        public AdminService(BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var totalUsersCount = await _context.Users.CountAsync();
            var totalAccountsCount = await _context.Accounts.CountAsync();
            var totalTransactionsCount = await _context.Transactions.CountAsync();
            var totalTransactedAmount = await _context.Transactions.SumAsync(t => t.Amount);

            return new AdminDashboardDto
            {
                Users = totalUsersCount,
                Accounts = totalAccountsCount,
                TransactedAmount = totalTransactedAmount,
                TransactionsCount = totalTransactionsCount
            };
        }
        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }
        public async Task<object> GetUserTransactionStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();

            var usersWithTransactions = await _context.Transactions
                .Select(t => t.Account.UserId)
                .Distinct()
                .CountAsync();

            var usersWithoutTransactions = totalUsers - usersWithTransactions;

            return new
            {
                TotalUsers = totalUsers,
                UsersWithTransactions = usersWithTransactions,
                UsersWithoutTransactions = usersWithoutTransactions
            };
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .ThenInclude(a => a.User)
                .ToListAsync();
        }

        public async Task DeactivateUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId)
                ?? throw new Exception("User not found");

            user.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
