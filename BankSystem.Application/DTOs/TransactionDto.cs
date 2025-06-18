using BankSystem.Domain.Entities;

namespace BankSystem.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
