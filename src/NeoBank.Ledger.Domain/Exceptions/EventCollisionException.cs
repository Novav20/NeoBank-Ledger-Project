namespace NeoBank.Ledger.Domain.Exceptions;

public sealed class EventCollisionException(long sequenceNumber)
    : InvalidOperationException($"The event log is immutable. Sequence number {sequenceNumber} already exists and cannot be overwritten.")
{
    public long SequenceNumber { get; } = sequenceNumber;
}