using BankSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Data;

public class BankSystemDbContext : DbContext
{
    public BankSystemDbContext() { }
    public BankSystemDbContext(DbContextOptions<BankSystemDbContext> options)
		: base(options) { }

	public DbSet<User> Users => Set<User>();
	public DbSet<Account> Accounts => Set<Account>();
	public DbSet<Transaction> Transactions => Set<Transaction>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Relationships
		modelBuilder.Entity<Account>()
			.HasOne(a => a.User)
			.WithMany()   // If User has `List<Account> Accounts`, use `.WithMany(u => u.Accounts)`
			.HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Account>()
			.Property(a => a.Balance) 
			.HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
			.HasOne(t => t.Account)
			.WithMany()
			.HasForeignKey(t => t.AccountId);

        modelBuilder.Entity<Transaction>()
			.Property(t => t.Amount)
			.HasPrecision(18, 2);
    }
}
