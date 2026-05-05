using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class EntryMapper
{
    public static EntryDto ToDto(this Entry entity) => new()
    {
        EntryId = entity.EntryId,
        TransactionId = entity.TransactionId,
        EventId = entity.EventId,
        AccountId = entity.AccountId,
        Side = entity.Side,
        AmountMinorUnits = entity.AmountMinorUnits,
        CurrencyCode = entity.CurrencyCode,
        BookedAt = entity.BookedAt,
        PostingOrder = entity.PostingOrder,
        ShardId = entity.ShardId
    };

    public static Entry ToEntity(this EntryDto dto) => new(
        dto.EntryId,
        dto.TransactionId,
        dto.EventId,
        dto.AccountId,
        dto.Side,
        dto.AmountMinorUnits,
        dto.CurrencyCode,
        dto.BookedAt,
        dto.PostingOrder,
        dto.ShardId);
}