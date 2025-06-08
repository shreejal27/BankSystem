using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly BankSystemDbContext _context;
        public UserService(BankSystemDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return false;

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return null;

            return $"fake-jwt-token-for-{user.Email}";
        }
    }
}
