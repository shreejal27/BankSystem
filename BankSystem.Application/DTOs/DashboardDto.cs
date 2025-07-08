namespace BankSystem.Application.DTOs
{
    public class DashboardDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal TotalBalance { get; set; }
        public IEnumerable<TransactionDto> RecentTransactions { get; set; } = new List<TransactionDto>();
    }
}
