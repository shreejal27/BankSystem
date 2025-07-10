namespace BankSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task DeactivateUserAsync(Guid userId);
    }
}
