using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.ValueObjects;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Ordered immutable log produced by the sequencer.
/// Grounded in HLD sequencing flow and ISO 20022 delivery order rules.
/// </summary>
public class Event(
    Guid eventId,
    long sequenceNumber,
    Guid correlationId,
    UniqueTransactionIdentifier uti,
    DateTimeOffset timestamp,
    TimestampPrecision precision,
    DeliveryOrder deliveryOrder,
    DeliveryAssurance deliveryAssurance,
    string payloadJson,
    bool nonRepudiationProof,
    string shardId,
    string consensusZoneId)
{
    public Guid EventId { get; init; } = eventId;
    public long SequenceNumber { get; init; } = sequenceNumber;
    public Guid CorrelationId { get; init; } = correlationId;
    public UniqueTransactionIdentifier UTI { get; init; } = uti;
    public DateTimeOffset Timestamp { get; init; } = timestamp;
    public TimestampPrecision Precision { get; init; } = precision;
    public DeliveryOrder DeliveryOrder { get; init; } = deliveryOrder;
    public DeliveryAssurance DeliveryAssurance { get; init; } = deliveryAssurance;
    public string PayloadJson { get; init; } = payloadJson;
    public bool NonRepudiationProof { get; init; } = nonRepudiationProof;
    public string ShardId { get; init; } = shardId;
    public string ConsensusZoneId { get; init; } = consensusZoneId;
}
