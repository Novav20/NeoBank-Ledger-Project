using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

/// <summary>
/// Persistence DTO for the Account entity.
/// </summary>
public class AccountDto
{
    public required Guid AccountId { get; set; }
    public required string OwnerLEI { get; set; }
    public required string AccountNumber { get; set; }
    public required string BaseCurrencyCode { get; set; }
    public required string PartitionKey { get; set; }
    public required string ShardId { get; set; }
    public required string ConsensusZoneId { get; set; }
    public required AccountStatus Status { get; set; }
    public required DateTimeOffset OpenedAt { get; set; }
    public DateTimeOffset? ClosedAt { get; set; }
}
