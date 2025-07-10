using BankSystem.Application.Interfaces;
using BankSystem.Infrastructure.Data;

namespace BankSystem.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly BankSystemDbContext _context;

        public AdminService(BankSystemDbContext context)
        {
            _context = context;
        }
        public async Task DeactivateUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId)
                ?? throw new Exception("User not found");

            user.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
