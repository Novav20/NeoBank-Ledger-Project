using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Reference data for LEI registry, target market eligibility, and whitelist checks.
/// Grounded in MiFID II Art. 10, Recital 71.
/// </summary>
public class Party(
    Guid partyId,
    string lei,
    string legalName,
    bool targetMarketEligible,
    PartyRole role)
{
    public Guid PartyId { get; init; } = partyId;
    public string LEI { get; init; } = lei;
    public string LegalName { get; init; } = legalName;
    public bool TargetMarketEligible { get; init; } = targetMarketEligible;
    public PartyRole Role { get; init; } = role;
    public string RegistrationStatus { get; private set; } = "Registered";
}
