using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankSystemDbContext _context;
        private readonly IEmailService _emailService;

        public TransactionService(BankSystemDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task DepositAsync(DepositDto dto)
        {
            var account = await _context.Accounts.FindAsync(dto.AccountId)
                ?? throw new Exception("Account not found");

            account.Balance += dto.Amount;

            _context.Transactions.Add(new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                Type = TransactionType.Deposit,
                Description = $"Deposited {dto.Amount}"
            });

            await _context.SaveChangesAsync();

            var user = account.User;
            if (user != null)
            {
                var subject = "Deposit Confirmation";
                var message = $"Hi {user.Name},\n\nYou've successfully deposited {dto.Amount} into your account.";
                await _emailService.SendEmailAsync(user.Email, subject, message);
            }
        }

        public async Task WithdrawAsync(WithdrawDto dto)
        {
            var account = await _context.Accounts.FindAsync(dto.AccountId)
                ?? throw new Exception("Account not found");

            if (account.Balance < dto.Amount)
                throw new Exception("Insufficient balance");

            account.Balance -= dto.Amount;

            _context.Transactions.Add(new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                Type = TransactionType.Withdraw,
                Description = $"Withdrew {dto.Amount}"
            });

            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(
               account.User.Email,
               "Withdrawal Confirmation",
               $"Dear {account.User.Name},<br/><br/>You have withdrawn <strong>{dto.Amount}</strong> from your account {account.AccountNumber}.<br/>Remaining Balance: <strong>{account.Balance}</strong><br/><br/>Regards,<br/>BankSystem"
           );
        }

        public async Task TransferAsync(TransferDto dto)
        {
            var fromAccount = await _context.Accounts.FindAsync(dto.FromAccountId)
                ?? throw new Exception("Sender account not found");

            var toAccount = await _context.Accounts.FindAsync(dto.ToAccountId)
                ?? throw new Exception("Receiver account not found");

            if (fromAccount.Balance < dto.Amount)
                throw new Exception("Insufficient balance");

            fromAccount.Balance -= dto.Amount;
            toAccount.Balance += dto.Amount;

            _context.Transactions.Add(new Transaction
            {
                AccountId = fromAccount.Id,
                Amount = dto.Amount,
                Type = TransactionType.Transfer,
                Description = $"Transferred {dto.Amount} to {toAccount.AccountNumber}"
            });

            _context.Transactions.Add(new Transaction
            {
                AccountId = toAccount.Id,
                Amount = dto.Amount,
                Type = TransactionType.Transfer,
                Description = $"Received {dto.Amount} from {fromAccount.AccountNumber}"
            });

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAccountTransactionsAsync(Guid accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();

            return transactions;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();

            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                AccountId = t.AccountId,
                Amount = t.Amount,
                Type = t.Type,
                CreatedAt = t.Timestamp
            });
        }
    }
}
