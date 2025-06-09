using BankSystem.Domain.Entities;

namespace BankSystem.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
