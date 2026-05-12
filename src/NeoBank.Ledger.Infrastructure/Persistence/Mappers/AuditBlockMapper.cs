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
        PreviousBlockHash = [.. entity.PreviousBlockHash],
        ChameleonHash = [.. entity.ChameleonHash],
        QuorumCert = [.. entity.QuorumCert],
        CommittedAt = entity.CommittedAt,
        ShardId = entity.ShardId,
        ConsensusZoneId = entity.ConsensusZoneId,
        RegistrationStatus = entity.RegistrationStatus,
        ChangeType = entity.ChangeType
    };

    public static AuditBlock ToEntity(this AuditBlockDto dto) => new(
        dto.AuditBlockId,
        dto.EventId,
        dto.BlockHeight,
        [.. dto.PreviousBlockHash],
        [.. dto.ChameleonHash],
        [.. dto.QuorumCert],
        dto.CommittedAt,
        dto.ShardId,
        dto.ConsensusZoneId,
        dto.RegistrationStatus,
        dto.ChangeType);
}