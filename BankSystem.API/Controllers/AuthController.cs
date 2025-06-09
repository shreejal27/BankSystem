using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
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
            return result ? Ok("User registered.") : BadRequest("Email already in use.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _jwt.GenerateToken(user);
            return Ok(new { Token = token });
        }

    }
}
