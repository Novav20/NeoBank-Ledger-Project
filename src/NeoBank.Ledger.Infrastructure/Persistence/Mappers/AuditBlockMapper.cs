using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class AuditBlockMapper
{
    public static AuditBlockDto ToDto(this AuditBlock entity) => new()
    {
        AuditBlockId = entity.AuditBlockId,
        EventId = entity.EventId,
        BlockHeight = entity.BlockHeight,
        PreviousBlockHash = entity.PreviousBlockHash.ToArray(),
        ChameleonHash = entity.ChameleonHash.ToArray(),
        QuorumCert = entity.QuorumCert.ToArray(),
        CommittedAt = entity.CommittedAt,
        ShardId = entity.ShardId,
        ConsensusZoneId = entity.ConsensusZoneId
    };

    public static AuditBlock ToEntity(this AuditBlockDto dto) => new(
        dto.AuditBlockId,
        dto.EventId,
        dto.BlockHeight,
        dto.PreviousBlockHash.ToArray(),
        dto.ChameleonHash.ToArray(),
        dto.QuorumCert.ToArray(),
        dto.CommittedAt,
        dto.ShardId,
        dto.ConsensusZoneId);
}