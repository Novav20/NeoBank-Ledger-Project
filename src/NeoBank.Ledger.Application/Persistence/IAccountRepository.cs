using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Application.Persistence;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
}