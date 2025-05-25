using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BankSystem.Infrastructure.Data
{
    public class BankSystemDbContextFactory : IDesignTimeDbContextFactory<BankSystemDbContext>
    {
        public BankSystemDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BankSystemDbContext>();

            // Replace with your actual connection string
            var connectionString = "Server=HELLOWORLD\\SQLEXPRESS;Database=BankSystemDb;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);

            return new BankSystemDbContext(optionsBuilder.Options);
        }
    }
}
