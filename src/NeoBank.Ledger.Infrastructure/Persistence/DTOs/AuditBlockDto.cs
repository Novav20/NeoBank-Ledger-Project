namespace NeoBank.Ledger.Infrastructure.Persistence.DTOs;

/// <summary>
/// Persistence DTO for the AuditBlock entity.
/// </summary>
public class AuditBlockDto
{
    public required Guid AuditBlockId { get; set; }
    public required Guid EventId { get; set; }
    public required long BlockHeight { get; set; }
    public required byte[] PreviousBlockHash { get; set; }
    public required byte[] ChameleonHash { get; set; }
    public required byte[] QuorumCert { get; set; }
    public required DateTimeOffset CommittedAt { get; set; }
    public required string ShardId { get; set; }
    public required string ConsensusZoneId { get; set; }
}