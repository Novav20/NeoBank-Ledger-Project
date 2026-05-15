using System.Text.RegularExpressions;

namespace NeoBank.Ledger.Domain.ValueObjects;

/// <summary>
/// Represents a Unique Transaction Identifier (UTI) as mandated by MiFID II Art 16.7.
/// LEI prefix plus a unique suffix, with a maximum length of 52 alphanumeric characters.
/// </summary>
public record UniqueTransactionIdentifier(string Value)
{
    private static readonly Regex UtiPattern = new("^[A-Z0-9]{21,52}$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public string Value { get; init; } = (string.IsNullOrWhiteSpace(Value) || Value.Length > 52)
        ? throw new ArgumentException("UTI must be between 21 and 52 alphanumeric characters.", nameof(Value))
        : Normalize(Value);

    private static string Normalize(string value)
    {
        string normalized = value.Trim().ToUpperInvariant();

        if (!UtiPattern.IsMatch(normalized))
        {
            throw new ArgumentException("UTI must be a 20-character LEI prefix followed by a unique alphanumeric suffix.", nameof(Value));
        }

        return normalized;
    }

    public static implicit operator string(UniqueTransactionIdentifier uti) => uti.Value;
}
