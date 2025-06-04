using BankSystem.Domain.Entities;
using BankSystem.Application.DTOs;

namespace BankSystem.Application.Interfaces
{
    public class IUserService
    {
        Task<bool> RegisterAsync(RegisterUserDto dto);
    }
}
