using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

/// <summary>
/// Persistence DTO for the RejectionRecord entity.
/// </summary>
public class RejectionRecordDto
{
    public required Guid RejectionId { get; set; }
    public required Guid TransactionId { get; set; }
    public required string UTI { get; set; }
    public required string ShardId { get; set; }
    public required string ConsensusZoneId { get; set; }
    public required ValidationLevel FailedAtLevel { get; set; }
    public required string Pacs002Code { get; set; }
    public required string ReasonCode { get; set; }
    public required string ReasonText { get; set; }
    public required DateTimeOffset RejectedAt { get; set; }
}