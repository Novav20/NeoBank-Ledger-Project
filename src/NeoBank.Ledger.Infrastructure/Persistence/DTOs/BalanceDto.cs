using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

/// <summary>
/// Persistence DTO for the Balance projection.
/// </summary>
public class BalanceDto
{
    public required Guid BalanceId { get; set; }
    public required Guid AccountId { get; set; }
    public required long LedgerMinorUnits { get; set; }
    public required long AvailableMinorUnits { get; set; }
    public required string CurrencyCode { get; set; }
    public required DateTimeOffset AsOfTimestamp { get; set; }
    public required long ProjectionVersion { get; set; }
    public required long LastAppliedSequenceNumber { get; set; }
    public required string ShardId { get; set; }
    public required RegistrationStatus RegistrationStatus { get; set; }
}
