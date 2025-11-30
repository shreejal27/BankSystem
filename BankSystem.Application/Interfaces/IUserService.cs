using BankSystem.Application.DTOs;
using BankSystem.Domain.Entities;

namespace BankSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterUserDto dto);
        Task<string?> LoginAsync(LoginUserDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task<IEnumerable<User>> GetAllUsersAsync(string flag);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<bool> UpdateUserAsync(Guid id, UserDto dto);
        Task<bool> ToggleUserStatusAsync(Guid id);
    }
}
