using System.Text.Json;
using NeoBank.Ledger.Domain.Entities;
using NeoBank.Ledger.Infrastructure.Persistence.DTOs;
using NeoBank.Ledger.Infrastructure.Persistence.Mappers;
using RocksDbNet;

namespace NeoBank.Ledger.Infrastructure.Persistence;

public sealed class RocksDbStore : IDisposable
{
    private readonly RocksDb _db;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.General);
    private long _lastSequence;

    public RocksDbStore(string? databasePath = null)
    {
        var resolvedPath = Path.GetFullPath(databasePath ?? Path.Combine(AppContext.BaseDirectory, "rocksdb"));
        Directory.CreateDirectory(resolvedPath);

        var options = new DbOptions { CreateIfMissing = true };

        try
        {
            _db = RocksDb.Open(options, resolvedPath);
            _lastSequence = LoadLastSequence();
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException($"Failed to open the RocksDB store at '{resolvedPath}'.", exception);
        }
    }

    internal RocksDb Database => _db;

    public void Dispose() => _db.Dispose();

    public void Save(Account account) => Put(BuildAccountKey(account.AccountId), account.ToDto());

    public void Save(Event eventEntity) => Put(BuildEventKey(eventEntity.SequenceNumber), eventEntity.ToDto());

    public void Save(Balance balance) => Put(BuildBalanceKey(balance.AccountId), balance.ToDto());

    public void Save(Party party) => Put(BuildPartyKey(party.PartyId), party.ToDto());

    public void Save(AuditBlock auditBlock) => Put(BuildAuditBlockKey(auditBlock.BlockHeight), auditBlock.ToDto());

    public void Save(RejectionRecord rejectionRecord) => Put(BuildRejectionRecordKey(rejectionRecord.UTI.Value), rejectionRecord.ToDto());

    public void Write(WriteBatch writeBatch)
    {
        var writeOptions = new WriteOptions();

        try
        {
            _db.Write(writeBatch, writeOptions);
        }
        finally
        {
            if (writeOptions is IDisposable disposableWriteOptions)
            {
                disposableWriteOptions.Dispose();
            }
        }
    }

    public Account? GetAccount(Guid accountId) => Get<AccountDto>(BuildAccountKey(accountId))?.ToEntity();

    public Transaction? GetTransaction(Guid transactionId) => Get<TransactionDto>(BuildTransactionKey(transactionId))?.ToEntity();

    public Entry? GetEntry(Guid transactionId, long postingOrder) => Get<EntryDto>(BuildEntryKey(transactionId, postingOrder))?.ToEntity();

    public Event? GetEvent(long sequenceNumber) => Get<EventDto>(BuildEventKey(sequenceNumber))?.ToEntity();

    public Balance? GetBalance(Guid accountId) => Get<BalanceDto>(BuildBalanceKey(accountId))?.ToEntity();

    public Party? GetParty(Guid partyId) => Get<PartyDto>(BuildPartyKey(partyId))?.ToEntity();

    public AuditBlock? GetAuditBlock(long blockHeight) => Get<AuditBlockDto>(BuildAuditBlockKey(blockHeight))?.ToEntity();

    public RejectionRecord? GetRejectionRecord(string uti) => Get<RejectionRecordDto>(BuildRejectionRecordKey(uti))?.ToEntity();

    internal long GetLastSequence() => Interlocked.Read(ref _lastSequence);

    internal void SetLastSequence(long seq) => Interlocked.Exchange(ref _lastSequence, seq);

    internal static string BuildTransactionKey(Guid transactionId) => $"txn:{transactionId}";

    internal static string BuildEntryKey(Guid transactionId, long postingOrder) => $"ent:{transactionId}:{postingOrder:D20}";

    internal static string BuildBalanceKey(Guid accountId) => $"bal:{accountId}";

    internal static string BuildAccountKey(Guid accountId) => $"acc:{accountId}";

    internal static string BuildEventKey(long sequenceNumber) => $"evt:{sequenceNumber:D20}";

    internal static string BuildPartyKey(Guid partyId) => $"pty:{partyId}";

    internal static string BuildAuditBlockKey(long blockHeight) => $"aud:{blockHeight:D20}";

    internal static string BuildRejectionRecordKey(string uti) => $"rej:{uti}";

    internal static string BuildLastSequenceKey() => "meta:last_sequence";

    private long LoadLastSequence()
    {
        string? rawValue = _db.GetString(BuildLastSequenceKey());
        return long.TryParse(rawValue, out long parsedValue) ? parsedValue : 0;
    }

    private void Put<TDto>(string key, TDto dto)
    {
        string json = JsonSerializer.Serialize(dto, _jsonOptions);
        _db.Put(key, json);
    }

    private TDto? Get<TDto>(string key) where TDto : class
    {
        string? json = _db.GetString(key);
        return string.IsNullOrWhiteSpace(json)
            ? null
            : JsonSerializer.Deserialize<TDto>(json, _jsonOptions);
    }
}