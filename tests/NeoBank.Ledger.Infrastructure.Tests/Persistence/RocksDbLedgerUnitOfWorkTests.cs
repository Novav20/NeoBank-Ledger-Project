using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.ValueObjects;
using NeoBank.Ledger.Infrastructure.Persistence;
using NeoBank.Ledger.Infrastructure.Persistence.Repositories;
using Xunit;

namespace NeoBank.Ledger.Infrastructure.Tests.Persistence;

public sealed class RocksDbLedgerUnitOfWorkTests : IDisposable
{
    private readonly string databasePath = CreateTemporaryDatabasePath();

    [Fact]
    public async Task CommitAsync_ShouldPersistAllEntitiesAtomicsally()
    {
        // Arrange
        using var rocksDbStore = new RocksDbStore(databasePath);
        var unitOfWork = new RocksDbLedgerUnitOfWork(rocksDbStore);

        Guid transactionId = Guid.NewGuid();
        Guid eventId = Guid.NewGuid();
        Guid debitAccountId = Guid.NewGuid();
        Guid creditAccountId = Guid.NewGuid();
        DateTimeOffset bookedAt = DateTimeOffset.UtcNow;

        Transaction transaction = new(
            transactionId,
            new UniqueTransactionIdentifier("UTI-20260512-0001"),
            "E2E-20260512-0001",
            new LegalEntityIdentifier("5493001KJTIIGC8Y1R12"),
            new LegalEntityIdentifier("5493001KJTIIGC8Y1R13"),
            "US1234567890",
            "pacs.008.001.10",
            MessageFunction.Newm,
            new CurrencyAmount(12500, "USD"),
            bookedAt,
            "partition-01",
            "shard-01",
            "cz-01");

        Entry[] entries =
        [
            new Entry(
                Guid.NewGuid(),
                transactionId,
                eventId,
                debitAccountId,
                EntrySide.Debit,
                12500,
                "USD",
                bookedAt,
                1,
                "shard-01"),
            new Entry(
                Guid.NewGuid(),
                transactionId,
                eventId,
                creditAccountId,
                EntrySide.Credit,
                12500,
                "USD",
                bookedAt,
                2,
                "shard-01")
        ];

        Balance[] balances =
        [
            new Balance(
                Guid.NewGuid(),
                debitAccountId,
                -12500,
                -12500,
                "USD",
                bookedAt,
                1,
                1,
                "shard-01",
                RegistrationStatus.Registered),
            new Balance(
                Guid.NewGuid(),
                creditAccountId,
                12500,
                12500,
                "USD",
                bookedAt,
                1,
                1,
                "shard-01",
                RegistrationStatus.Registered)
        ];

        string transactionKey = RocksDbStore.BuildTransactionKey(transactionId);
        string firstEntryKey = RocksDbStore.BuildEntryKey(transactionId, 1);
        string secondEntryKey = RocksDbStore.BuildEntryKey(transactionId, 2);
        string debitBalanceKey = RocksDbStore.BuildBalanceKey(debitAccountId);
        string creditBalanceKey = RocksDbStore.BuildBalanceKey(creditAccountId);

        // Act
        await unitOfWork.CommitAsync(transaction, balances, entries);

        // Assert
        Assert.StartsWith("txn:", transactionKey);
        Assert.StartsWith("ent:", firstEntryKey);
        Assert.StartsWith("ent:", secondEntryKey);
        Assert.StartsWith("bal:", debitBalanceKey);
        Assert.StartsWith("bal:", creditBalanceKey);

        Assert.Equal(transactionKey, RocksDbStore.BuildTransactionKey(transactionId));
        Assert.Equal(firstEntryKey, RocksDbStore.BuildEntryKey(transactionId, 1));
        Assert.Equal(secondEntryKey, RocksDbStore.BuildEntryKey(transactionId, 2));

        Assert.NotNull(rocksDbStore.Database.GetString(transactionKey));
        Assert.NotNull(rocksDbStore.Database.GetString(firstEntryKey));
        Assert.NotNull(rocksDbStore.Database.GetString(secondEntryKey));
        Assert.NotNull(rocksDbStore.Database.GetString(debitBalanceKey));
        Assert.NotNull(rocksDbStore.Database.GetString(creditBalanceKey));

        Transaction? persistedTransaction = rocksDbStore.GetTransaction(transactionId);
        Entry? persistedFirstEntry = rocksDbStore.GetEntry(transactionId, 1);
        Entry? persistedSecondEntry = rocksDbStore.GetEntry(transactionId, 2);
        Balance? persistedDebitBalance = rocksDbStore.GetBalance(debitAccountId);
        Balance? persistedCreditBalance = rocksDbStore.GetBalance(creditAccountId);

        Assert.NotNull(persistedTransaction);
        Assert.NotNull(persistedFirstEntry);
        Assert.NotNull(persistedSecondEntry);
        Assert.NotNull(persistedDebitBalance);
        Assert.NotNull(persistedCreditBalance);

        Assert.Equal(transaction.Amount.MinorUnits, persistedTransaction!.Amount.MinorUnits);
        Assert.Equal(transaction.Amount.CurrencyCode, persistedTransaction.Amount.CurrencyCode);
        Assert.Equal(MessageFunction.Newm, persistedTransaction.MessageFunction);

        Assert.Equal(entries[0].AmountMinorUnits, persistedFirstEntry!.AmountMinorUnits);
        Assert.Equal(entries[0].Side, persistedFirstEntry.Side);
        Assert.Equal(entries[1].AmountMinorUnits, persistedSecondEntry!.AmountMinorUnits);
        Assert.Equal(entries[1].Side, persistedSecondEntry.Side);

        Assert.Equal(balances[0].LedgerMinorUnits, persistedDebitBalance!.LedgerMinorUnits);
        Assert.Equal(balances[0].AvailableMinorUnits, persistedDebitBalance.AvailableMinorUnits);
        Assert.Equal(balances[1].LedgerMinorUnits, persistedCreditBalance!.LedgerMinorUnits);
        Assert.Equal(balances[1].AvailableMinorUnits, persistedCreditBalance.AvailableMinorUnits);
    }

    public void Dispose()
    {
        if (Directory.Exists(databasePath))
        {
            Directory.Delete(databasePath, recursive: true);
        }
    }

    private static string CreateTemporaryDatabasePath()
    {
        string path = Path.Combine(Path.GetTempPath(), "NeoBank.Ledger.Infrastructure.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }
}