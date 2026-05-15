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

    public static CurrencyAmount operator +(CurrencyAmount left, CurrencyAmount right) => Combine(left, right, 1);

    public static CurrencyAmount operator -(CurrencyAmount left, CurrencyAmount right) => Combine(left, right, -1);

    private static CurrencyAmount Combine(CurrencyAmount left, CurrencyAmount right, int sign)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        if (!string.Equals(left.CurrencyCode, right.CurrencyCode, StringComparison.Ordinal))
        {
            throw new ArgumentException("Currency codes must match before arithmetic can be performed.", nameof(right));
        }

        return new CurrencyAmount(left.MinorUnits + (sign * right.MinorUnits), left.CurrencyCode);
    }

    public override string ToString() => $"{MinorUnits} {CurrencyCode}";
}
