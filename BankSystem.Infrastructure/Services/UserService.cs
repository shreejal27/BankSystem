using BankSystem.Application.Common.Utils;
using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Enums;
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
                IsActive = null,
                Password = newGeneratedPassword,
            };
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(
              user.Email,
               "Registration Received – Awaiting Approval",

                        $"Dear {user.Email},<br/><br/>" +
                        "Thank you for registering with BankSystem!<br/>" +
                        $"Your account request has been successfully received and is currently <strong>pending approval</strong> from our administration team.<br/><br/>" +
                        $"Once your account is reviewed and approved, you will receive another email containing your: <br/>" +
                        $"<strong>Account Number</strong><br/>" +
                        $" <strong>Temporary Password</strong><br/><br/>" +

                        "Thank you for choosing BankSystem.We will notify you as soon as your account is activated.<br/><br/>" +
                        "Regards,<br/>BankSystem Team");
            return true;
        }

        public async Task<ServiceResponseDto> LoginAsync(LoginUserDto dto)
        {
            var response = new ServiceResponseDto();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User Not Found!";
                return response;
            }

            if (user.IsActive == null)
            {
                response.Success = false;
                response.Message = "User account is not activated";
                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                response.Success = false;
                response.Message = "Invalid Credentials";
                return response;
            }

            var token = _jwt.GenerateToken(user);
            response.Success = true;
            response.Message = "Login successful";
            response.Data = token;

            return response;
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

        public async Task<IEnumerable<User>> GetAllUsersAsync(string flag)
        {
            var query = _context.Users.Where(u => u.Role == Role.User);

            if (flag?.ToLower() == "all")
            {
                query = query.Where(u => u.IsActive == true || u.IsActive == false);
            }
            else if (flag?.ToLower() == "pending")
            {
                query = query.Where(u => u.IsActive == null);
            }

            return await query.OrderByDescending(u => u.CreatedAt).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<bool> UpdateUserAsync(Guid id, UserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Name = dto.Name;
            user.Email = dto.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ToggleUserStatusAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = !user.IsActive;
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
