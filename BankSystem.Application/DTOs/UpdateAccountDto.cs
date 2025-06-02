namespace BankSystem.Application.DTOs
{
    public class UpdateAccountDto
    {
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
    }
}
