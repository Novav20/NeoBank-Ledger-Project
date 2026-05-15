using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.Services;
using NeoBank.Ledger.Domain.ValueObjects;
using Xunit;

namespace NeoBank.Ledger.Infrastructure.Tests.Domain;

public sealed class DoubleEntryEngineTests
{
    [Fact]
    public void CurrencyAmount_ShouldAddAndSubtractMatchingCurrencies()
    {
        CurrencyAmount opening = new(1_000, "usd");
        CurrencyAmount adjustment = new(250, "USD");

        CurrencyAmount sum = opening + adjustment;
        CurrencyAmount difference = sum - adjustment;

        Assert.Equal(1_250, sum.MinorUnits);
        Assert.Equal("USD", sum.CurrencyCode);
        Assert.Equal(opening, difference);
    }

    [Fact]
    public void CurrencyAmount_ShouldThrow_WhenCurrenciesDoNotMatch()
    {
        CurrencyAmount usd = new(1_000, "USD");
        CurrencyAmount eur = new(500, "EUR");

        ArgumentException exception = Assert.Throws<ArgumentException>(() => _ = usd + eur);

        Assert.Contains("match", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ValueObjects_ShouldNormalizeAndValidateStandards()
    {
        UniqueTransactionIdentifier uti = new("5493001kjtiigc8y1r12txn01");
        LegalEntityIdentifier lei = new("5493001kjtiigc8y1r12");
        ISIN isin = new("us1234567890");

        Assert.Equal("5493001KJTIIGC8Y1R12TXN01", uti.Value);
        Assert.Equal("5493001KJTIIGC8Y1R12", lei.Value);
        Assert.Equal("US1234567890", isin.Value);
    }

    [Fact]
    public void Transaction_ShouldTrimIdentifiers_AndRejectNonPositiveAmount()
    {
        Transaction transaction = new(
            Guid.NewGuid(),
            new UniqueTransactionIdentifier("5493001KJTIIGC8Y1R12TXN02"),
            "  E2E-20260515-0001  ",
            new LegalEntityIdentifier("5493001KJTIIGC8Y1R12"),
            new LegalEntityIdentifier("5493001KJTIIGC8Y1R13"),
            new ISIN("US1234567890"),
            "  pacs.008.001.10  ",
            MessageFunction.Newm,
            new CurrencyAmount(1_000, "USD"),
            DateTimeOffset.UtcNow,
            "  partition-01  ",
            "  shard-01  ",
            "  cz-01  ");

        Assert.Equal("E2E-20260515-0001", transaction.EndToEndId);
        Assert.Equal("pacs.008.001.10", transaction.MessageDefinitionId);
        Assert.Equal("partition-01", transaction.PartitionKey);
        Assert.Equal("shard-01", transaction.ShardId);
        Assert.Equal("cz-01", transaction.ConsensusZoneId);

        ArgumentException exception = Assert.Throws<ArgumentException>(() => new Transaction(
            Guid.NewGuid(),
            new UniqueTransactionIdentifier("5493001KJTIIGC8Y1R12TXN03"),
            "E2E-20260515-0002",
            new LegalEntityIdentifier("5493001KJTIIGC8Y1R12"),
            new LegalEntityIdentifier("5493001KJTIIGC8Y1R13"),
            new ISIN("US1234567890"),
            "pacs.008.001.10",
            MessageFunction.Newm,
            new CurrencyAmount(0, "USD"),
            DateTimeOffset.UtcNow,
            "partition-01",
            "shard-01",
            "cz-01"));

        Assert.Contains("greater than zero", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DoubleEntryEngine_ShouldValidateBalancedEntries()
    {
        IDoubleEntryEngine engine = new DoubleEntryEngine();
        DateTimeOffset bookedAt = DateTimeOffset.UtcNow;
        Guid transactionId = Guid.NewGuid();
        Guid eventId = Guid.NewGuid();

        Entry[] balancedEntries =
        [
            new Entry(Guid.NewGuid(), transactionId, eventId, Guid.NewGuid(), EntrySide.Debit, 500, "USD", bookedAt, 1, "shard-01"),
            new Entry(Guid.NewGuid(), transactionId, eventId, Guid.NewGuid(), EntrySide.Credit, 500, "USD", bookedAt, 2, "shard-01")
        ];

        DoubleEntryValidationResult result = engine.Validate(balancedEntries);

        Assert.True(result.IsValid);
        Assert.NotNull(result.NetAmount);
        Assert.Equal(0, result.NetAmount!.MinorUnits);
        Assert.Equal("USD", result.NetAmount.CurrencyCode);
    }

    [Fact]
    public void DoubleEntryEngine_ShouldInvalidateUnbalancedEntries()
    {
        IDoubleEntryEngine engine = new DoubleEntryEngine();
        DateTimeOffset bookedAt = DateTimeOffset.UtcNow;
        Guid transactionId = Guid.NewGuid();
        Guid eventId = Guid.NewGuid();

        Entry[] unbalancedEntries =
        [
            new Entry(Guid.NewGuid(), transactionId, eventId, Guid.NewGuid(), EntrySide.Debit, 500, "USD", bookedAt, 1, "shard-01"),
            new Entry(Guid.NewGuid(), transactionId, eventId, Guid.NewGuid(), EntrySide.Credit, 400, "USD", bookedAt, 2, "shard-01")
        ];

        DoubleEntryValidationResult result = engine.Validate(unbalancedEntries);

        Assert.False(result.IsValid);
        Assert.NotNull(result.NetAmount);
        Assert.Equal(-100, result.NetAmount!.MinorUnits);
        Assert.Equal("USD", result.NetAmount.CurrencyCode);
    }
}