using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class BalanceMapper
{
    public static BalanceDto ToDto(this Balance entity) => new()
    {
        BalanceId = entity.BalanceId,
        AccountId = entity.AccountId,
        LedgerMinorUnits = entity.LedgerMinorUnits,
        AvailableMinorUnits = entity.AvailableMinorUnits,
        CurrencyCode = entity.CurrencyCode,
        AsOfTimestamp = entity.AsOfTimestamp,
        ProjectionVersion = entity.ProjectionVersion,
        LastAppliedSequenceNumber = entity.LastAppliedSequenceNumber,
        ShardId = entity.ShardId,
        RegistrationStatus = entity.RegistrationStatus
    };

    public static Balance ToEntity(this BalanceDto dto) => new(
        dto.BalanceId,
        dto.AccountId,
        dto.LedgerMinorUnits,
        dto.AvailableMinorUnits,
        dto.CurrencyCode,
        dto.AsOfTimestamp,
        dto.ProjectionVersion,
        dto.LastAppliedSequenceNumber,
        dto.ShardId,
        dto.RegistrationStatus);
}