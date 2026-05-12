using NeoBank.Ledger.Application.Persistence;
using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Infrastructure.Persistence.Repositories;

public sealed class RocksDbAccountRepository(RocksDbStore rocksDbStore) : IAccountRepository
{
    public Task<Account?> GetByIdAsync(Guid id) => Task.FromResult(rocksDbStore.GetAccount(id));
}