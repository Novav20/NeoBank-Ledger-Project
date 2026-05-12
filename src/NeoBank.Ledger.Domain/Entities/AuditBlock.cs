using NeoBank.Ledger.Domain.Enums;

namespace NeoBank.Ledger.Domain.Entities;

/// <summary>
/// Immutable evidence bundle and hash-chain anchor.
/// Grounded in ADR-001 and ADR-002.
/// </summary>
public record AuditBlock(
    Guid AuditBlockId,
    Guid EventId,
    long BlockHeight,
    byte[] PreviousBlockHash,
    byte[] ChameleonHash,
    byte[] QuorumCert,
    DateTimeOffset CommittedAt,
    string ShardId,
    string ConsensusZoneId,
    RegistrationStatus RegistrationStatus = RegistrationStatus.Registered,
    ChangeType ChangeType = ChangeType.Create);
