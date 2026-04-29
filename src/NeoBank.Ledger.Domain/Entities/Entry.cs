using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Double-entry line item and immutable posting record.
/// Grounded in ISO 20022 business element rules and BPA 5.2.
/// </summary>
public record Entry(
    Guid EntryId,
    Guid TransactionId,
    Guid EventId,
    Guid AccountId,
    EntrySide Side,
    long AmountMinorUnits,
    string CurrencyCode,
    DateTimeOffset BookedAt,
    long PostingOrder,
    string ShardId);
