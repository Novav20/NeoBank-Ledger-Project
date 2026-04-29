using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.ValueObjects;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Business anchor for balance ownership and shard partitioning.
/// Grounded in ISO 20022 composite aggregation rules and BPA 5.2.
/// </summary>
public class Account(
    Guid accountId,
    LegalEntityIdentifier ownerLei,
    string accountNumber,
    string baseCurrencyCode,
    string partitionKey,
    string shardId,
    string consensusZoneId,
    AccountStatus status,
    DateTimeOffset openedAt)
{
    public Guid AccountId { get; init; } = accountId;
    public LegalEntityIdentifier OwnerLEI { get; init; } = ownerLei;
    public string AccountNumber { get; init; } = accountNumber;
    public string BaseCurrencyCode { get; init; } = baseCurrencyCode;
    public string PartitionKey { get; init; } = partitionKey;
    public string ShardId { get; init; } = shardId;
    public string ConsensusZoneId { get; init; } = consensusZoneId;
    public AccountStatus Status { get; private set; } = status;
    public DateTimeOffset OpenedAt { get; init; } = openedAt;
    public DateTimeOffset? ClosedAt { get; private set; }

    public void Close(DateTimeOffset closedAt)
    {
        Status = AccountStatus.Closed;
        ClosedAt = closedAt;
    }

    public void Suspend() => Status = AccountStatus.Suspended;
    public void Reactivate() => Status = AccountStatus.Active;
}
