using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Application.Persistence;

public interface ILedgerUnitOfWork
{
    Task CommitAsync(
        Transaction transaction,
        IReadOnlyCollection<Balance> updatedBalances,
        IReadOnlyCollection<Entry>? entries = null);
}