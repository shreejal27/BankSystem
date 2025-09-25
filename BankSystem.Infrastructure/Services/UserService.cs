using BankSystem.Application.Common.Utils;
using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankSystem.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly BankSystemDbContext _context;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public UserService(BankSystemDbContext context, IJwtTokenGenerator jwt, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _jwt = jwt;
            _emailService = emailService;
            _config = config;
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
        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;

            var jwtSettings = _config.GetSection("JwtSettings");
            var key = jwtSettings["Key"];

            try
            {
                var principal = handler.ValidateToken(dto.Token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateLifetime = true
                }, out validatedToken);

                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                if (email == null)
                    return false;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                    return false;

                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
