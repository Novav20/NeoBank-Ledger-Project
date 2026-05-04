using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.ValueObjects;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class EventMapper
{
    public static EventDto ToDto(this Event entity) => new()
    {
        EventId = entity.EventId,
        SequenceNumber = entity.SequenceNumber,
        CorrelationId = entity.CorrelationId,
        UTI = entity.UTI,
        Timestamp = entity.Timestamp,
        Precision = entity.Precision,
        DeliveryOrder = entity.DeliveryOrder,
        DeliveryAssurance = entity.DeliveryAssurance,
        PayloadJson = entity.PayloadJson,
        NonRepudiationProof = entity.NonRepudiationProof,
        ShardId = entity.ShardId,
        ConsensusZoneId = entity.ConsensusZoneId
    };

    public static Event ToEntity(this EventDto dto) => new(
        dto.EventId,
        dto.SequenceNumber,
        dto.CorrelationId,
        new UniqueTransactionIdentifier(dto.UTI),
        dto.Timestamp,
        dto.Precision,
        dto.DeliveryOrder,
        dto.DeliveryAssurance,
        dto.PayloadJson,
        dto.NonRepudiationProof,
        dto.ShardId,
        dto.ConsensusZoneId);
}