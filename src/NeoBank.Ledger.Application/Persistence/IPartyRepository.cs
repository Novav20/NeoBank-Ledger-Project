using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Application.Persistence;

public interface IPartyRepository
{
    Task<Party?> GetByIdAsync(Guid id);
}