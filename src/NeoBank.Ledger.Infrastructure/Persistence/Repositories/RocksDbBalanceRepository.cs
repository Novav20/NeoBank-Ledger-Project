using NeoBank.Ledger.Application.Persistence;
using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Infrastructure.Persistence.Repositories;

public sealed class RocksDbBalanceRepository(RocksDbStore rocksDbStore) : IBalanceRepository
{
    public Task<Balance?> GetByAccountIdAsync(Guid accountId) => Task.FromResult(rocksDbStore.GetBalance(accountId));
}