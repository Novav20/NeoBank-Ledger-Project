# Naming Conventions

## 1. Directories
- **Format**: lowercase-kebab-case.
- **Prefix**: Numeric prefix (00_, 01_, 02_) for root documentation folders to enforce logical ordering.
- **Example**: `00_meta`, `02_analysis`, `orchestration`, `api-contracts`.
- **Week Log Folders**: Prefer lowercase `wNN` format (e.g., `w11`, `w12`) for weekly folders under logs/plans.

## 2. Files
- **Formal Documents**: PascalCase.
    - **Description**: High-level specifications, designs, and architectural records.
    - **Example**: `Requirements.md`, `SystemDesign.md`, `NamingConventions.md`.
- **Logs, Plans & Templates**: lowercase-kebab-case.
    - **Description**: Ephemeral or tracking data, daily logs, and templates.
    - **Example**: `session-state.md`, `w11-setup-plan.md`, `copilot-instruction.md`.

## 3. General Rules
- **No Emojis**: All documentation and internal communication must be strictly text-based.
- **Language**: English.
- **Special Characters**: Avoid spaces or non-standard characters in filenames. Use hyphens (`-`) for kebab-case.
- **Definition of Done**: Artifact completion criteria are defined in `docs/00_meta/DefinitionOfDone.md`.
