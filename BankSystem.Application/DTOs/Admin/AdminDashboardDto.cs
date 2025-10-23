namespace BankSystem.Application.DTOs.Admin
{
    public class AdminDashboardDto
    {
        public int Users {  get; set; }
        public int Accounts { get; set; }
        public decimal TransactedAmount { get; set; }
        public int TransactionsCount { get; set; }
    }
}
