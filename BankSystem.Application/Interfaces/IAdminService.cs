﻿using BankSystem.Domain.Entities;

namespace BankSystem.Application.Interfaces
{
    public interface IAdminService
    {
        Task<int> GetTotalUsersAsync();
        Task<object> GetUserTransactionStatsAsync();
        Task DeactivateUserAsync(Guid userId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
    }
}
