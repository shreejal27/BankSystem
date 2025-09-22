using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly BankSystemDbContext _context;
        private readonly IJwtTokenGenerator _jwt;
        public AuthController(IUserService userService, BankSystemDbContext context, IJwtTokenGenerator jwt)
        {
            _userService = userService;
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (result)
            {
                return StatusCode(201, new
                {
                    statusCode = 201,
                    message = "User successfully registered.",
                });
            }

            return StatusCode(409, new
            {
                statusCode = 409,
                message = "Email already exists."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);
            var token = await _userService.LoginAsync(dto);
            if (token == null)
            {
                return StatusCode(401, new
                {
                    statusCode = 401,
                    message = "Invalid credentials"
                });
            }

            return StatusCode(200, new
            {
                statusCode = 200,
                message = "Login successful",
                data = new
                {
                    token
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(
                new {
                         message = "User logged out successfully." 
                    }
            );
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _userService.ResetPasswordAsync(dto.TempPassword, dto.NewPassword);
            if (!result)
                return BadRequest(new { message = "Invalid or expired temporary password." });

            return Ok(new { message = "Password reset successfully." });
        }


    }
}
