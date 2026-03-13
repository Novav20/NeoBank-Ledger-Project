# Neobank Ledger API

High-performance, auditable ledger backend for B2B FinTech use cases, built with C# 14 and .NET 10.

## Why This Project
- **Auditability-first**: Ledger design emphasizes traceability and immutable history where possible.
- **High transaction volume**: Architecture targets consistency and throughput under financial workloads.
- **Clean Architecture**: Clear separation between domain, application, infrastructure, and API layers.

## Tech Stack
- **Runtime**: .NET 10
- **Language**: C# 14
- **Persistence**: SQL Server + EF Core 10
- **Serialization**: `System.Text.Json`

## Repository Layout
- **src/**: API and application implementation.
- **tests/**: Unit and integration tests.
- **docs/**: Requirements, architecture artifacts, ADRs, and project governance.

## Current Status
This repository is in active architecture and implementation planning. The documentation model and governance workflow are established, and implementation progresses incrementally from ADR-driven decisions.

## Documentation
- **Requirements and analysis**: `docs/02_analysis/`
- **Architecture artifacts**: `docs/03_architecture/`
- **Architecture Decision Records (ADRs)**: `docs/00_meta/adr/`

## Contributing
For contribution and maintainer workflow details, see `CONTRIBUTING.md`.
