using System.Text.RegularExpressions;

namespace NeoBank.Ledger.Domain.ValueObjects;

/// <summary>
/// Represents an International Securities Identification Number (ISIN) as mandated by ISO 6166.
/// Exactly 12 characters in the country-code plus instrument-identifier format.
/// </summary>
public record ISIN(string Value)
{
    private static readonly Regex IsinPattern = new("^[A-Z]{2}[A-Z0-9]{9}[0-9]$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public string Value { get; init; } = Normalize(Value);

    private static string Normalize(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        string normalized = value.Trim().ToUpperInvariant();

        if (!IsinPattern.IsMatch(normalized))
        {
            throw new ArgumentException("ISIN must be exactly 12 characters in ISO 6166 format.", nameof(Value));
        }

        return normalized;
    }

    public static implicit operator string(ISIN isin) => isin.Value;
}