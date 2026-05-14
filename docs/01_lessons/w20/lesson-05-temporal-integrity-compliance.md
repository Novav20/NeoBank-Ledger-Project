# Lesson 20.05: Temporal Integrity and MiFID II Compliance

In a high-integrity ledger, the "Time" a transaction occurs is a legal fact. If we lose precision, we can't prove the order of events, which leads to regulatory failure.

## 1. What is MiFID II (RTS 25)?
**MiFID II** (Markets in Financial Instruments Directive) is an European regulation. Its **RTS 25** (Regulatory Technical Standard) defines exactly how financial systems must record time.

- **The Rule**: For most electronic trading, the system must synchronize clocks and record events with at least **1-millisecond (1ms)** precision.
- **The Challenge**: Standard `DateTime` in many languages is not precise enough or changes based on the server's time zone.

---

## 2. Our Solution: `DateTimeOffset` and UTC
We use **`DateTimeOffset`** instead of `DateTime`.

| Type | Why we use it |
| :--- | :--- |
| **DateTime** | Ambiguous. "12:00 PM" could be New York or London time. |
| **DateTimeOffset** | Absolute. It includes the offset from UTC (e.g., `2026-05-14T18:00:00+00:00`). |

**The Golden Rule**: All timestamps in the NeoBank Ledger are converted to **UTC (Z)** before being saved. This ensures that no matter where the server is located, the timeline is global and consistent.

---

## 3. Serialization Round-Tripping
When we save an event to RocksDB, it goes through this "Lifecycle":
1.  **C# Object**: `DateTimeOffset` (High precision in memory).
2.  **JSON String**: `2026-05-14T18:06:01.1234567Z` (Serialized).
3.  **RocksDB**: Saved as Bytes on disk.
4.  **Retrieval**: Bytes -> String -> C# Object.

**The Danger Zone**: If the JSON serializer is configured poorly, it might round `1.123ms` to `1.0ms`. We would lose the 1ms precision required by law.

---

## 4. How we Validate Precision
Today, we will write a **Precision Verification Test**. 

**The Test logic**:
1.  Create a timestamp with exactly `555` milliseconds.
2.  Save the entity to RocksDB.
3.  Read it back.
4.  Assert that the result still has exactly `555` milliseconds.

If the assertion passes, we have mathematically proven our **Regulatory Compliance** for Sprint 01.

---

**Next Step**: Review **ADR-011** to see the formal decision on high-precision serialization.
