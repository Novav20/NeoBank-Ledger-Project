using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.ValueObjects;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Intake aggregate root and legal record of intent.
/// Grounded in ISO 20022 business transaction rules and MiFID II reporting.
/// </summary>
public class Transaction(
    Guid transactionId,
    UniqueTransactionIdentifier uti,
    string endToEndId,
    LegalEntityIdentifier initiatorLei,
    LegalEntityIdentifier counterpartyLei,
    ISIN isin,
    string messageDefinitionId,
    MessageFunction messageFunction,
    CurrencyAmount amount,
    DateTimeOffset eventTimestamp,
    string partitionKey,
    string shardId,
    string consensusZoneId)
{
    public Guid TransactionId { get; init; } = transactionId;
    public UniqueTransactionIdentifier UTI { get; init; } = uti; //TODO: Validate format
    public string EndToEndId { get; init; } = NormalizeIdentifier(endToEndId, nameof(endToEndId));
    public LegalEntityIdentifier InitiatorLEI { get; init; } = initiatorLei;  //TODO: Validate format
    public LegalEntityIdentifier CounterpartyLEI { get; init; } = counterpartyLei;  //TODO: Validate format
    public ISIN ISIN { get; init; } = isin;
    public string MessageDefinitionId { get; init; } = NormalizeIdentifier(messageDefinitionId, nameof(messageDefinitionId));
    public MessageFunction MessageFunction { get; init; } = messageFunction;
    public CurrencyAmount Amount { get; init; } = amount is null
        ? throw new ArgumentNullException(nameof(amount))
        : amount.MinorUnits <= 0
            ? throw new ArgumentException("Transaction amount must be greater than zero.", nameof(amount))
            : amount;
    public DateTimeOffset EventTimestamp { get; init; } = eventTimestamp;
    public ValidationLevel ValidationLevel { get; private set; } = ValidationLevel.NoValidation;
    public TransactionStatus Status { get; private set; } = TransactionStatus.Received;
    public string PartitionKey { get; init; } = NormalizeIdentifier(partitionKey, nameof(partitionKey));
    public string ShardId { get; init; } = NormalizeIdentifier(shardId, nameof(shardId));
    public string ConsensusZoneId { get; init; } = NormalizeIdentifier(consensusZoneId, nameof(consensusZoneId));

    public void UpdateValidationLevel(ValidationLevel newLevel)
    {
        ValidationLevel = newLevel;
    }

    public void MarkAsValidated() => Status = TransactionStatus.Validated;
    public void Reject() => Status = TransactionStatus.Rejected;
    public void Post() => Status = TransactionStatus.Posted;
    public void Settle() => Status = TransactionStatus.Settled;

    private static string NormalizeIdentifier(string value, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
        return value.Trim();
    }
}
