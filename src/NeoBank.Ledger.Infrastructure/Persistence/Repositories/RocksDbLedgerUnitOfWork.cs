using System.Text.Json;
using NeoBank.Ledger.Application.Persistence;
using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;
using NeoBank.Ledger.Infrastructure.Persistence.Mappers;
using RocksDbNet;

namespace NeoBank.Ledger.Infrastructure.Persistence.Repositories;

public sealed class RocksDbLedgerUnitOfWork(RocksDbStore rocksDbStore) : ILedgerUnitOfWork
{
    private readonly JsonSerializerOptions  jsonOptions = new(JsonSerializerDefaults.General);

    public Task CommitAsync(
        Transaction transaction,
        IReadOnlyCollection<Balance> updatedBalances,
        IReadOnlyCollection<Entry>? entries = null)
    {
        return Task.Run(() =>
        {
            var batch = new WriteBatch();

            try
            {
                AppendTransaction(batch, transaction);

                if (entries is not null)
                {
                    AppendEntries(batch, entries);
                }

                AppendBalances(batch, updatedBalances);
                rocksDbStore.Write(batch);
            }
            finally
            {
                if (batch is IDisposable disposableBatch)
                {
                    disposableBatch.Dispose();
                }
            }
        });
    }

    private void AppendTransaction(WriteBatch batch, Transaction transaction)
    {
        TransactionDto dto = transaction.ToDto();
        batch.Put(RocksDbStore.BuildTransactionKey(transaction.TransactionId), JsonSerializer.Serialize(dto, jsonOptions));
    }

    private void AppendEntries(WriteBatch batch, IReadOnlyCollection<Entry> entries)
    {
        foreach (Entry entry in entries)
        {
            EntryDto dto = entry.ToDto();
            batch.Put(RocksDbStore.BuildEntryKey(entry.TransactionId, entry.PostingOrder), JsonSerializer.Serialize(dto, jsonOptions));
        }
    }

    private void AppendBalances(WriteBatch batch, IReadOnlyCollection<Balance> balances)
    {
        foreach (Balance balance in balances)
        {
            BalanceDto dto = balance.ToDto();
            batch.Put(RocksDbStore.BuildBalanceKey(balance.AccountId), JsonSerializer.Serialize(dto, jsonOptions));
        }
    }
}