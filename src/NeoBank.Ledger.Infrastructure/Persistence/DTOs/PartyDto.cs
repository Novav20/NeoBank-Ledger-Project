using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

/// <summary>
/// Persistence DTO for the Party entity.
/// </summary>
public class PartyDto
{
    public required Guid PartyId { get; set; }
    public required string LEI { get; set; }
    public required string LegalName { get; set; }
    public required bool TargetMarketEligible { get; set; }
    public required PartyRole Role { get; set; }
    public required string RegistrationStatus { get; set; }
}