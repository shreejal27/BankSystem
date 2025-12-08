using BankSystem.Application.Common.Utils;
using BankSystem.Application.DTOs;
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
        private readonly IEmailService _emailService;

        public AdminService(BankSystemDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var totalUsersCount = await _context.Users.CountAsync();
            var totalAccountsCount = await _context.Accounts.CountAsync();
            var totalTransactionsCount = await _context.Transactions.CountAsync();
            var totalTransactedAmount = await _context.Transactions.SumAsync(t => t.Amount);

            var latestUsers = await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(7) 
            .Select(u => new AdminDashboardRecentUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

            var latestTransactions = await _context.Transactions
                .OrderByDescending(t => t.Timestamp)
                .Take(7) 
                .Select(t => new AdminDashboardRecentTransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type.ToString(),
                    Name = t.Account.User.Name,
                    Email = t.Account.User.Email,
                    Timestamp = t.Timestamp
                })
                .ToListAsync();

            return new AdminDashboardDto
            {
                Users = totalUsersCount,
                Accounts = totalAccountsCount,
                TransactedAmount = totalTransactedAmount,
                TransactionsCount = totalTransactionsCount,
                LatestUsers = latestUsers,
                LatestTransactions = latestTransactions
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

        public async Task ActivateUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId)
                ?? throw new Exception("User not found");

            user.IsActive = true;

            var userAccountInfo = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == user.Id) ?? throw new Exception("User Account not found");

            var decryptedAccountNumber = EncryptDecryptAccountNumber.Decrypt(userAccountInfo.AccountNumber);

            await _emailService.SendEmailAsync(
              user.Email,
               "Registration Successful",

                        $"Dear {user.Email},<br/><br/>" +
                        "Welcome to BankSystem!<br/>" +
                        $"Your account has been successfully created.<br/><br/>" +
                        $"Email: <strong>{user.Email}</strong><br/>" +
                        $"Password: <strong>{user.Password}</strong><br/>" +
                        $"AccountNumber: <strong>{decryptedAccountNumber}</strong><br/><br/>" +
                        "Please keep your credentials safe and change your password after logging in for the first time.<br/><br/>" +
                        "Regards,<br/>BankSystem");


            await _context.SaveChangesAsync();
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
