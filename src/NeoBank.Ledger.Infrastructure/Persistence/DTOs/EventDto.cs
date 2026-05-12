using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

/// <summary>
/// Persistence DTO for the Event entity.
/// </summary>
public class EventDto
{
    public required Guid EventId { get; set; }
    public required long SequenceNumber { get; set; }
    public required Guid CorrelationId { get; set; }
    public required string UTI { get; set; }
    public required DateTimeOffset Timestamp { get; set; }
    public required TimestampPrecision Precision { get; set; }
    public required DeliveryOrder DeliveryOrder { get; set; }
    public required DeliveryAssurance DeliveryAssurance { get; set; }
    public required string PayloadJson { get; set; }
    public required bool NonRepudiationProof { get; set; }
    public required string ShardId { get; set; }
    public required string ConsensusZoneId { get; set; }
}
