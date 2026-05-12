using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;

namespace NeoBank.Ledger.Infrastructure.Persistence.Mappers;

public static class PartyMapper
{
    public static PartyDto ToDto(this Party entity) => new()
    {
        PartyId = entity.PartyId,
        LEI = entity.LEI,
        LegalName = entity.LegalName,
        TargetMarketEligible = entity.TargetMarketEligible,
        Role = entity.Role,
        RegistrationStatus = entity.RegistrationStatus
    };

    public static Party ToEntity(this PartyDto dto) => new(
        dto.PartyId,
        dto.LEI,
        dto.LegalName,
        dto.TargetMarketEligible,
        dto.Role,
        dto.RegistrationStatus);
}