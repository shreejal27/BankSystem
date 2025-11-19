using BankSystem.Domain.Entities;

namespace BankSystem.Application.DTOs.Admin
{
    public class AdminDashboardRecentTransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}