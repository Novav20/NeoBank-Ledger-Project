using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

public class TransactionDto
{
    public required Guid TransactionId { get; set; }
    public required string UTI { get; set; }
    public required string EndToEndId { get; set; }
    public required string InitiatorLEI { get; set; }
    public required string CounterpartyLEI { get; set; }
    public required string ISIN { get; set; }
    public required string MessageDefinitionId { get; set; }
    public required MessageFunction MessageFunction { get; set; }
    public required long AmountMinorUnits { get; set; }
    public required string AmountCurrencyCode { get; set; }
    public required DateTimeOffset EventTimestamp { get; set; }
    public required ValidationLevel ValidationLevel { get; set; }
    public required TransactionStatus Status { get; set; }
    public required string PartitionKey { get; set; }
    public required string ShardId { get; set; }
    public required string ConsensusZoneId { get; set; }
}