namespace NeoBank.Ledger.Domain.ValueObjects;

/// <summary>
/// Represents a monetary value in its smallest unit (e.g., cents for USD).
/// Enforces mathematical exactness by using long (BigInt) instead of float/decimal.
/// </summary>
public record CurrencyAmount(long MinorUnits, string CurrencyCode)
{
    public string CurrencyCode { get; init; } = (string.IsNullOrWhiteSpace(CurrencyCode) || CurrencyCode.Length != 3)
        ? throw new ArgumentException("Currency code must be a valid 3-letter ISO code.", nameof(CurrencyCode))
        : CurrencyCode.ToUpperInvariant();

    public override string ToString() => $"{MinorUnits} {CurrencyCode}";
}
