namespace NeoBank.Ledger.Domain.ValueObjects;

/// <summary>
/// Represents a Unique Transaction Identifier (UTI) as mandated by MiFID II Art 16.7.
/// Max 52 alphanumeric characters.
/// </summary>
public record UniqueTransactionIdentifier(string Value)
{
    public string Value { get; init; } = (string.IsNullOrWhiteSpace(Value) || Value.Length > 52)
        ? throw new ArgumentException("UTI must be between 1 and 52 characters.", nameof(Value))
        : Value;

    public static implicit operator string(UniqueTransactionIdentifier uti) => uti.Value;
}
