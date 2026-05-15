using System.Text.RegularExpressions;

namespace NeoBank.Ledger.Domain.ValueObjects;

/// <summary>
/// Represents a Legal Entity Identifier (LEI) as mandated by ISO 17442.
/// Exactly 20 alphanumeric characters.
/// </summary>
public record LegalEntityIdentifier(string Value)
{
    private static readonly Regex LeiPattern = new("^[A-Z0-9]{20}$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public string Value { get; init; } = (string.IsNullOrWhiteSpace(Value) || Value.Length != 20)
        ? throw new ArgumentException("LEI must be exactly 20 alphanumeric characters.", nameof(Value))
        : Normalize(Value);

    private static string Normalize(string value)
    {
        string normalized = value.Trim().ToUpperInvariant();

        if (!LeiPattern.IsMatch(normalized))
        {
            throw new ArgumentException("LEI must be exactly 20 alphanumeric characters.", nameof(Value));
        }

        return normalized;
    }

    public static implicit operator string(LegalEntityIdentifier lei) => lei.Value;
}
