using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.ValueObjects;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class RejectionRecordMapper
{
    public static RejectionRecordDto ToDto(this RejectionRecord entity) => new()
    {
        RejectionId = entity.RejectionId,
        TransactionId = entity.TransactionId,
        UTI = entity.UTI,
        ShardId = entity.ShardId,
        ConsensusZoneId = entity.ConsensusZoneId,
        FailedAtLevel = entity.FailedAtLevel,
        Pacs002Code = entity.Pacs002Code,
        ReasonCode = entity.ReasonCode,
        ReasonText = entity.ReasonText,
        RejectedAt = entity.RejectedAt
    };

    public static RejectionRecord ToEntity(this RejectionRecordDto dto) => new(
        dto.RejectionId,
        dto.TransactionId,
        new UniqueTransactionIdentifier(dto.UTI),
        dto.ShardId,
        dto.ConsensusZoneId,
        dto.FailedAtLevel,
        dto.Pacs002Code,
        dto.ReasonCode,
        dto.ReasonText,
        dto.RejectedAt);
}