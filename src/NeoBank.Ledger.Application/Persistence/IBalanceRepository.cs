using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Application.Persistence;

public interface IBalanceRepository
{
    Task<Balance?> GetByAccountIdAsync(Guid accountId);
}