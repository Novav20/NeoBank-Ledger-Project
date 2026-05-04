using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.ValueObjects;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Negative acknowledgement for failed validation or compliance checks.
/// Grounded in ISO 20022 rejection semantics and pacs.002 mapping.
/// </summary>
public record RejectionRecord(
    Guid RejectionId,
    Guid TransactionId,
    UniqueTransactionIdentifier UTI,
    string ShardId,
    string ConsensusZoneId,
    ValidationLevel FailedAtLevel,
    string Pacs002Code,
    string ReasonCode,
    string ReasonText,
    DateTimeOffset RejectedAt);
