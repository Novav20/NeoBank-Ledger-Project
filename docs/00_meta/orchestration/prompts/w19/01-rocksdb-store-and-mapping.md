---
status: pending
issued_by: Gemini CLI
issued_date: 2026-05-04
response_ref: docs/00_meta/orchestration/responses/w19/01-rocksdb-store-and-mapping.md
---
# Task: Implement Static Mappers and RocksDbStore

## Task Context
We are implementing the RocksDB persistence engine for the NeoBank Ledger, pivoting away from LevelDB (as per ADR-008). We are also using pure C# Static Extension Methods for mapping between Domain Entities and Persistence DTOs, avoiding AutoMapper (as per ADR-007).

## Objectives
1.  **Update Dependencies**: Remove `LevelDB.Net` from the Infrastructure project and install `RocksDb.Net`.
2.  **Create Static Mappers**: Create a `Mappers` directory in the Infrastructure layer and implement static extension methods to convert Domain Entities to DTOs and vice-versa.
3.  **Create RocksDbStore**: Create the `RocksDbStore.cs` class. Open the database in the constructor and implement the `Save` and `Get` methods for all entities using JSON serialization and ADR-008 key schemas.

## Constraints & Requirements
- **Language**: C# 14 / .NET 10.
- **Mapping Pattern**: Use `public static class [Entity]Mapper` with `public static [Entity]Dto ToDto(this [Entity] entity)` and `public static [Entity] ToEntity(this [Entity]Dto dto)`.
- **RocksDB Syntax**: Use `RocksDb.Open`, `db.Put`, and `db.Get` (using `RocksDb.Net` features).
- **Null Safety**: Handle cases where keys do not exist safely without throwing unhandled exceptions.
- **JSON**: Use `System.Text.Json.JsonSerializer`.

## Specific Instructions
1.  **Mapper Location**: Create files like `src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/AccountMapper.cs`.
2.  **Value Object Handling**: When mapping Entities to DTOs, extract the primitive values from Value Objects (e.g., `entity.OwnerLEI.Value` or `.ToString()`). When mapping DTOs to Entities, reconstruct the Value Objects.
3.  **RocksDbStore Constructor**: Use a try/catch block around `RocksDb.Open`. If it throws an exception (e.g., file lock), wrap it in a custom or standard exception with a clear message.
4.  **Key Formatting**: Ensure keys use the exact prefixes from ADR-008 (e.g., `evt:{SequenceNumber:D20}`, `acc:{AccountId}`, `bal:{AccountId}`, `pty:{PartyId}`, `aud:{BlockHeight:D20}`, `rej:{UTI}`).

## Implementation Path
- `src/NeoBank.Ledger.Infrastructure/Persistence/Mappers/` (New Folder & Files)
- `src/NeoBank.Ledger.Infrastructure/Persistence/RocksDbStore.cs` (New File)
