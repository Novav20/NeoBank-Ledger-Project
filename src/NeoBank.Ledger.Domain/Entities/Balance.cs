using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Materialized read model derived from validated entries.
/// Grounded in BPA 5.2 Conceptual Static / Balance Projection.
/// </summary>
public class Balance(
    Guid balanceId,
    Guid accountId,
    long ledgerMinorUnits,
    long availableMinorUnits,
    string currencyCode,
    DateTimeOffset asOfTimestamp,
    long projectionVersion,
    long lastAppliedSequenceNumber,
    string shardId,
    RegistrationStatus registrationStatus = RegistrationStatus.Registered)
{
    public Guid BalanceId { get; init; } = balanceId;
    public Guid AccountId { get; init; } = accountId;
    public long LedgerMinorUnits { get; private set; } = ledgerMinorUnits;
    public long AvailableMinorUnits { get; private set; } = availableMinorUnits;
    public string CurrencyCode { get; init; } = currencyCode;
    public DateTimeOffset AsOfTimestamp { get; private set; } = asOfTimestamp;
    public long ProjectionVersion { get; private set; } = projectionVersion;
    public long LastAppliedSequenceNumber { get; private set; } = lastAppliedSequenceNumber;
    public string ShardId { get; init; } = shardId;
    public RegistrationStatus RegistrationStatus { get; init; } = registrationStatus;

    public void ApplyEntry(long amount, DateTimeOffset timestamp, long sequenceNumber)
    {
        if (sequenceNumber <= LastAppliedSequenceNumber) return;

        LedgerMinorUnits += amount;
        AvailableMinorUnits += amount;
        AsOfTimestamp = timestamp;
        LastAppliedSequenceNumber = sequenceNumber;
        ProjectionVersion++;
    }
}
