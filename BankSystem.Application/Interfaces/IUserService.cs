using BankSystem.Application.DTOs;

namespace BankSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterUserDto dto);
    }
}
