using BankSystem.Application.Common.Utils;
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
        private readonly IJwtTokenGenerator _jwt;
        private readonly IEmailService _emailService;

        public UserService(BankSystemDbContext context, IJwtTokenGenerator jwt, IEmailService emailService)
        {
            _context = context;
            _jwt = jwt;
            _emailService = emailService;
        }

        public async Task<bool> RegisterAsync(RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return false;

            var newGeneratedPassword = PasswordGenerator.GenerateRandomPassword(10);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(newGeneratedPassword)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            await _emailService.SendEmailAsync(
              user.Email,
               "Registration Successful",

                        $"Dear {user.Email},<br/><br/>" +
                        "Welcome to BankSystem!<br/>" +
                        "Your account has been successfully created.<br/><br/>" +
                        $"Email: <strong>{user.Email}</strong><br/>" +
                        $"Password: <strong>{newGeneratedPassword}</strong><br/><br/>" +
                        "Please keep your credentials safe and change your password after logging in for the first time.<br/><br/>" +
                        "Regards,<br/>BankSystem");


            return true;
        }

        public async Task<string?> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return null;

            var token = _jwt.GenerateToken(user);
            return token;
        }
    }
}
