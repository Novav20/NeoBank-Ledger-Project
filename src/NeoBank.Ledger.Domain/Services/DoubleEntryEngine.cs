using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Domain.Enums;
using NeoBank.Ledger.Domain.ValueObjects;

namespace NeoBank.Ledger.Domain.Services;

public interface IDoubleEntryEngine
{
    DoubleEntryValidationResult Validate(IEnumerable<Entry> entries);
}

public sealed class DoubleEntryEngine : IDoubleEntryEngine
{
    public DoubleEntryValidationResult Validate(IEnumerable<Entry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        using IEnumerator<Entry> enumerator = entries.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return new DoubleEntryValidationResult(false, null);
        }

        CurrencyAmount total = ToSignedAmount(enumerator.Current);

        while (enumerator.MoveNext())
        {
            total += ToSignedAmount(enumerator.Current);
        }

        return new DoubleEntryValidationResult(total.MinorUnits == 0, total);
    }

    private static CurrencyAmount ToSignedAmount(Entry entry) => entry.Side == EntrySide.Debit
        ? new CurrencyAmount(-entry.AmountMinorUnits, entry.CurrencyCode)
        : new CurrencyAmount(entry.AmountMinorUnits, entry.CurrencyCode);
}

public sealed record DoubleEntryValidationResult(bool IsValid, CurrencyAmount? NetAmount);