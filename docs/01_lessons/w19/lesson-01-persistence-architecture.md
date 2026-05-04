# Lesson 19.01: Persistence Architecture (EF Core vs RocksDB & Mapping)

## 1. The Database Paradigm Shift: EF Core vs. RocksDB

When transitioning from standard enterprise applications to high-throughput financial ledgers, the persistence strategy changes drastically.

### EF Core & SQL Server (The Relational Model)
- **Concept**: Data is stored in 2D tables with strict schemas and foreign keys.
- **Tooling**: Entity Framework Core acts as an ORM, translating C# objects into SQL queries and tracking changes in memory.
- **Pros**: Excellent for complex querying (`JOIN`, `GROUP BY`), reporting, and general-purpose CRUD.
- **Cons**: Write-heavy operations suffer because relational databases use B-Trees, which require random disk I/O to update indexes.

### RocksDB (The Key-Value Model)
- **Concept**: Data is stored as a massive dictionary of `Key -> Value` pairs on disk.
- **Tooling**: `RocksDb.Net` provides direct access to the database engine. There is no ORM. You serialize objects to bytes (or JSON) and store them against a string key.
- **Syntax**: 
  - Write: `db.Put(key, value)`
  - Read: `db.Get(key)`
- **Pros**: Blisteringly fast for appending new records (events) due to its LSM-Tree architecture and multi-threaded compaction. Perfect for Event Sourcing.
- **Cons**: You cannot perform SQL queries. To find a user by email, you either need the exact key or you must build a secondary index manually.

---

## 2. The Tale of Two DTOs (Where do they live?)

In Clean Architecture, DTO (Data Transfer Object) is a generic term. Their placement depends entirely on what boundary they are crossing.

### Application Layer DTOs (API/Web)
- **Purpose**: Moving data between the API controllers (HTTP) and the Application Use Cases.
- **Examples**: `CreateTransferCommand`, `AccountBalanceResponse`.
- **Location**: Application Layer.

### Infrastructure Layer DTOs (Persistence)
- **Purpose**: Moving data between the Application's Repository Interfaces and the physical database.
- **Examples**: `AccountDto`, `EventDto`.
- **Location**: Infrastructure Layer.
- **Why?**: The Application layer operates entirely on Rich Domain Entities (e.g., `Transaction`). It does not know the database exists. The Infrastructure layer acts as an adapter, taking the pure `Transaction` entity and squashing it into a `TransactionDto` that RocksDB can easily serialize to JSON.

---

## 3. Mapping Strategy: Why we avoid AutoMapper

In the past, libraries like AutoMapper were standard for converting Entities to DTOs. However, in modern .NET development (especially in 2026 contexts), we avoid them for high-performance ledgers:

1. **Performance**: AutoMapper traditionally relies on Reflection, which introduces runtime overhead.
2. **Maintainability**: "Magic" mapping hides errors. If a property name changes, the mapping might silently fail at runtime instead of throwing a compile-time error.
3. **Dependencies**: Minimizing external dependencies reduces licensing risks and supply-chain vulnerabilities.

### The Solution: Static Extension Methods
We map objects manually using pure, statically typed C# methods. 

```csharp
// Example Mapping Extension
public static class AccountMapper
{
    public static AccountDto ToDto(this Account entity)
    {
        return new AccountDto
        {
            AccountId = entity.AccountId,
            OwnerLEI = entity.OwnerLEI.Value, // Flattening a Value Object
            Status = entity.Status
        };
    }
}
```
**Benefits**: 
- **Zero Overhead**: It runs as fast as native C# execution.
- **Compile-Time Safety**: If you rename a property in `Account`, the compiler will immediately break the `AccountMapper`, forcing you to fix it before the app runs.
