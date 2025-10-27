namespace BankSystem.Application.Common.Utils
{
    public class AccountNumberGenerator
    {
        public static string GenerateAccountNumber()
        {
            var random = new Random();
            string accountNumber = "999";

            for (int i = 0; i < 17; i++)
            {
                accountNumber += random.Next(0, 10); 
            }

            return accountNumber;
        }
    }
}
