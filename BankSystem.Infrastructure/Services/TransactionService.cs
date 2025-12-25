using BankSystem.Application.Common.Utils;
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

        private const decimal LowBalanceThreshold = 100;

        public TransactionService(BankSystemDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task DepositAsync(DepositDto dto)
        {
            if (dto.Amount <= 0)
            {
                throw new Exception("Invalid deposit amount");
            }

            var encryptAccountNumber = EncryptDecryptAccountNumber.Encrypt(dto.AccountNumber);

            var account = await _context.Accounts.Include(a => a.User).FirstOrDefaultAsync(a => a.AccountNumber == encryptAccountNumber)
                ?? throw new Exception("Account not found");

            account.Balance += dto.Amount;

            _context.Transactions.Add(new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                Type = TransactionType.Deposit,
                Description = $"{account.User.Name} deposited {dto.Amount}"
            });

            await _context.SaveChangesAsync();

            var user = account.User;
            if (user != null && user.NeedEmail)
            {
                var subject = "Deposit Confirmation";
                var message = $"Hi {user.Name},\n\nYou've successfully deposited {dto.Amount} into your account.";
                await _emailService.SendEmailAsync(user.Email, subject, message);
            }
        }

        public async Task WithdrawAsync(WithdrawDto dto)
        {
            if (dto.Amount <= 0)
            {
                throw new Exception("Invalid withdraw amount");
            }
            var encryptAccountNumber = EncryptDecryptAccountNumber.Encrypt(dto.AccountNumber);

            var account = await _context.Accounts.Include(a => a.User).FirstOrDefaultAsync(a => a.AccountNumber == encryptAccountNumber)
             ?? throw new Exception("Account not found");

            if (account.Balance < dto.Amount)
                throw new Exception("Insufficient balance");

            account.Balance -= dto.Amount;

            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = dto.Amount,
                Type = TransactionType.Withdraw,
                Timestamp = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            if (account.User.NeedEmail)
            {
                await _emailService.SendEmailAsync(
                   account.User.Email,
                   "Withdrawal Confirmation",
                   $"Dear {account.User.Name},<br/><br/>You have withdrawn <strong>{dto.Amount}</strong> from your account.<br/>" +
                   $"Remaining Balance: <strong>{account.Balance}</strong><br/><br/>Regards,<br/>BankSystem"
               );
            }

            if (account.Balance < LowBalanceThreshold && account.User.NeedEmail)
            {
                await _emailService.SendEmailAsync(
                    account.User.Email,
                    "Low Balance Alert",
                    $"Dear {account.User.Name},<br/><br/>Your account balance is <strong>{account.Balance}</strong> which is below the safe threshold.<br/>Consider depositing funds to avoid service interruptions.<br/><br/>Regards,<br/>BankSystem"
                );
            }
        }

        public async Task TransferAsync(TransferDto dto)
        {
            if (dto.Amount <= 0)
            {
                throw new Exception("Invalid transfer amount");
            }

            if (dto.FromAccountNumber == dto.ToAccountNumber)
                throw new Exception("Cannot transfer to the same account.");

            var encryptFromAccountNumber = EncryptDecryptAccountNumber.Encrypt(dto.FromAccountNumber);

            var encryptToAccountNumber = EncryptDecryptAccountNumber.Encrypt(dto.ToAccountNumber);

            var fromAccount = await _context.Accounts.Include(a => a.User).FirstOrDefaultAsync(a => a.AccountNumber == encryptFromAccountNumber)
                ?? throw new Exception("Sender Account not found");

            var toAccount = await _context.Accounts.Include(a => a.User).FirstOrDefaultAsync(a => a.AccountNumber == encryptToAccountNumber)
              ?? throw new Exception("Receiver Account not found");

            if (fromAccount.Balance < dto.Amount)
                throw new Exception("Insufficient balance");

            fromAccount.Balance -= dto.Amount;
            toAccount.Balance += dto.Amount;

            var transactionOut = new Transaction
            {
                AccountId = fromAccount.Id,
                Amount = dto.Amount,
                Type = TransactionType.TransferOutgoing,
                Timestamp = DateTime.UtcNow,
                Description = $"{fromAccount.User.Name} transferred {dto.Amount} to {toAccount.User.Name}"
            };

            var transactionIn = new Transaction
            {
                AccountId = toAccount.Id,
                Amount = dto.Amount,
                Type = TransactionType.TransferIncoming,
                Timestamp = DateTime.UtcNow,
                Description = $"{toAccount.User.Name} received {dto.Amount} from {fromAccount.User.Name}"
            };

            _context.Transactions.AddRange(transactionOut, transactionIn);

            await _context.SaveChangesAsync();

            if (fromAccount.User.NeedEmail)
            {
                await _emailService.SendEmailAsync(
                   fromAccount.User.Email,
                   "Transfer Confirmation",
                   $"Dear {fromAccount.User.Name},<br/><br/>You have transferred <strong>{dto.Amount}</strong> to account ending with {dto.FromAccountNumber[^4..]}.<br/>Remaining Balance: <strong>{fromAccount.Balance}</strong><br/><br/>Regards,<br/>BankSystem"
               );

                if (fromAccount.Balance < LowBalanceThreshold)
                {
                    await _emailService.SendEmailAsync(
                        fromAccount.User.Email,
                        "Low Balance Alert",
                        $"Dear {fromAccount.User.Name},<br/><br/>After transferring {dto.Amount}, your remaining balance is <strong>{fromAccount.Balance}</strong>, which is below the safe threshold.<br/>Please consider depositing funds.<br/><br/>Regards,<br/>BankSystem"
                    );
                }
            }

            if (toAccount.User.NeedEmail)
            {
                await _emailService.SendEmailAsync(
                toAccount.User.Email,
                "Deposit Received",
                $"Dear {toAccount.User.Name},<br/><br/>You have received <strong>{dto.Amount}</strong> from account ending with {dto.ToAccountNumber[^4..]}.<br/>New Balance: <strong>{toAccount.Balance}</strong><br/><br/>Regards,<br/>BankSystem"
            );
            }
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
