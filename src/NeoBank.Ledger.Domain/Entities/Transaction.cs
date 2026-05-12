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
    string isin,
    string messageDefinitionId,
    MessageFunction messageFunction,
    CurrencyAmount amount,
    DateTimeOffset eventTimestamp,
    string partitionKey,
    string shardId,
    string consensusZoneId)
{
    public Guid TransactionId { get; init; } = transactionId;
    public UniqueTransactionIdentifier UTI { get; init; } = uti;
    public string EndToEndId { get; init; } = endToEndId;
    public LegalEntityIdentifier InitiatorLEI { get; init; } = initiatorLei;
    public LegalEntityIdentifier CounterpartyLEI { get; init; } = counterpartyLei;
    public string ISIN { get; init; } = isin;
    public string MessageDefinitionId { get; init; } = messageDefinitionId;
    public MessageFunction MessageFunction { get; init; } = messageFunction;
    public CurrencyAmount Amount { get; init; } = amount;
    public DateTimeOffset EventTimestamp { get; init; } = eventTimestamp;
    public ValidationLevel ValidationLevel { get; private set; } = ValidationLevel.NoValidation;
    public TransactionStatus Status { get; private set; } = TransactionStatus.Received;
    public string PartitionKey { get; init; } = partitionKey;
    public string ShardId { get; init; } = shardId;
    public string ConsensusZoneId { get; init; } = consensusZoneId;

    public void UpdateValidationLevel(ValidationLevel newLevel)
    {
        ValidationLevel = newLevel;
    }

    public void MarkAsValidated() => Status = TransactionStatus.Validated;
    public void Reject() => Status = TransactionStatus.Rejected;
    public void Post() => Status = TransactionStatus.Posted;
    public void Settle() => Status = TransactionStatus.Settled;
}
