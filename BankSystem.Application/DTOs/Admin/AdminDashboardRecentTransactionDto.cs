using BankSystem.Domain.Entities;

namespace BankSystem.Application.DTOs.Admin
{
    public class AdminDashboardRecentTransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string AccountNumber { get; set; }
        public string? Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}