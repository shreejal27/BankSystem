namespace BankSystem.Application.Interfaces
{
    public class IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
