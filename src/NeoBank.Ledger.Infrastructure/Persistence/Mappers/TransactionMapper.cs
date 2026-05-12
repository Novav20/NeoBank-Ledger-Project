using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.ValueObjects;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class TransactionMapper
{
    public static TransactionDto ToDto(this Transaction entity) => new()
    {
        TransactionId = entity.TransactionId,
        UTI = entity.UTI,
        EndToEndId = entity.EndToEndId,
        InitiatorLEI = entity.InitiatorLEI,
        CounterpartyLEI = entity.CounterpartyLEI,
        ISIN = entity.ISIN,
        MessageDefinitionId = entity.MessageDefinitionId,
        MessageFunction = entity.MessageFunction,
        AmountMinorUnits = entity.Amount.MinorUnits,
        AmountCurrencyCode = entity.Amount.CurrencyCode,
        EventTimestamp = entity.EventTimestamp,
        ValidationLevel = entity.ValidationLevel,
        Status = entity.Status,
        PartitionKey = entity.PartitionKey,
        ShardId = entity.ShardId,
        ConsensusZoneId = entity.ConsensusZoneId
    };

    public static Transaction ToEntity(this TransactionDto dto)
    {
        var entity = new Transaction(
            dto.TransactionId,
            new UniqueTransactionIdentifier(dto.UTI),
            dto.EndToEndId,
            new LegalEntityIdentifier(dto.InitiatorLEI),
            new LegalEntityIdentifier(dto.CounterpartyLEI),
            dto.ISIN,
            dto.MessageDefinitionId,
            dto.MessageFunction,
            new CurrencyAmount(dto.AmountMinorUnits, dto.AmountCurrencyCode),
            dto.EventTimestamp,
            dto.PartitionKey,
            dto.ShardId,
            dto.ConsensusZoneId);

        entity.UpdateValidationLevel(dto.ValidationLevel);

        if (dto.Status == TransactionStatus.Validated)
        {
            entity.MarkAsValidated();
        }
        else if (dto.Status == TransactionStatus.Rejected)
        {
            entity.Reject();
        }
        else if (dto.Status == TransactionStatus.Posted)
        {
            entity.Post();
        }
        else if (dto.Status == TransactionStatus.Settled)
        {
            entity.Settle();
        }

        return entity;
    }
}