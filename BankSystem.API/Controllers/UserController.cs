using BankSystem.Application.DTOs;
using BankSystem.Application.Interfaces;
using BankSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] string flag)
        {
            var users = await _userService.GetAllUsersAsync(flag);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto dto)
        {
            var success = await _userService.UpdateUserAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("toggle-status/{id}")]
        public async Task<IActionResult> ToggleUserStatusAsync(Guid id)
        {
            var success = await _userService.ToggleUserStatusAsync(id);
            return success ? NoContent() : NotFound();
        }

    }
}