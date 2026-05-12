using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.ValueObjects;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class AccountMapper
{
    public static AccountDto ToDto(this Account entity) => new()
    {
        AccountId = entity.AccountId,
        OwnerLEI = entity.OwnerLEI,
        AccountNumber = entity.AccountNumber,
        BaseCurrencyCode = entity.BaseCurrencyCode,
        PartitionKey = entity.PartitionKey,
        ShardId = entity.ShardId,
        ConsensusZoneId = entity.ConsensusZoneId,
        Status = entity.Status,
        OpenedAt = entity.OpenedAt,
        ClosedAt = entity.ClosedAt
    };

    public static Account ToEntity(this AccountDto dto)
    {
        var entity = new Account(
            dto.AccountId,
            new LegalEntityIdentifier(dto.OwnerLEI),
            dto.AccountNumber,
            dto.BaseCurrencyCode,
            dto.PartitionKey,
            dto.ShardId,
            dto.ConsensusZoneId,
            dto.Status,
            dto.OpenedAt);

        if (dto.ClosedAt is DateTimeOffset closedAt)
        {
            entity.Close(closedAt);
        }

        return entity;
    }
}