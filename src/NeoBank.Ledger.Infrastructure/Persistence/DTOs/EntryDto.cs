using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

public class EntryDto
{
    public required Guid EntryId { get; set; }
    public required Guid TransactionId { get; set; }
    public required Guid EventId { get; set; }
    public required Guid AccountId { get; set; }
    public required EntrySide Side { get; set; }
    public required long AmountMinorUnits { get; set; }
    public required string CurrencyCode { get; set; }
    public required DateTimeOffset BookedAt { get; set; }
    public required long PostingOrder { get; set; }
    public required string ShardId { get; set; }
}