using NeoBank.Ledger.Domain.Entities;

namespace NeoBank.Ledger.Application.Persistence;

public interface IEventStore
{
    Task<long> GetNextSequenceAsync();

    Task<Event> AppendAsync(Event eventEntity);

    Task<IEnumerable<Event>> GetEventsAsync(long start, long end);
}