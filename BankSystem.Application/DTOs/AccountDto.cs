namespace BankSystem.Application.DTOs
{
    public class AccountDto
    {
        public string AccountNumber { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
