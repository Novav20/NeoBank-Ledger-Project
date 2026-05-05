using NeoBank.Ledger.Application.Persistence;
using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Infrastructure.Persistence.Repositories;

public sealed class RocksDbPartyRepository(RocksDbStore rocksDbStore) : IPartyRepository
{
    public Task<Party?> GetByIdAsync(Guid id) => Task.FromResult(rocksDbStore.GetParty(id));
}