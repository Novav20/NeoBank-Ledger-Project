namespace NeoBank.Ledger.Domain.ValueObjects;

/// <summary>
/// Represents a Legal Entity Identifier (LEI) as mandated by ISO 17442.
/// Exactly 20 alphanumeric characters.
/// </summary>
public record LegalEntityIdentifier(string Value)
{
    public string Value { get; init; } = (string.IsNullOrWhiteSpace(Value) || Value.Length != 20)
        ? throw new ArgumentException("LEI must be exactly 20 characters.", nameof(Value))
        : Value.ToUpperInvariant();

    public static implicit operator string(LegalEntityIdentifier lei) => lei.Value;
}
