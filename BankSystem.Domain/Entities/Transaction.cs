namespace BankSystem.Domain.Entities
{
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer
    }

    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; } 
        public TransactionType Type { get; set; }  
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Account? Account { get; set; }
        public string? Description { get; set; }
    }
}
