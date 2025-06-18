using BankSystem.Application.DTOs;

namespace BankSystem.Application.Interfaces
{
    public interface ITransactionService
    {
        Task DepositAsync(DepositDto dto);
        Task WithdrawAsync(WithdrawDto dto);
        Task TransferAsync(TransferDto dto);
        Task<IEnumerable<Transaction>> GetAccountTransactionsAsync(Guid accountId);
        Task<IEnumerable<TransactionDto>> GetTransactionsByAccountIdAsync(Guid accountId);
    }
}
