namespace BankSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task<int> GetTotalUsersAsync();
        Task DeactivateUserAsync(Guid userId);
    }
}
