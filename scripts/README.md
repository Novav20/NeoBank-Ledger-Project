# Scripts

This folder contains utility scripts used for research ingestion and repository maintenance.

Keep instructions concise and follow the examples below when running scripts.

Prerequisites
- Python 3.8+ available on PATH
- (Optional) a virtual environment at the repository root: `.venv`

Quick setup
```bash
# create venv (if not present)
python3 -m venv .venv
# activate the venv (Linux/macOS)
source .venv/bin/activate
# deactivate when done
deactivate
```

extract_batch_snippets.py
- Purpose: split a raw batch markdown file containing fenced code blocks into individual `.md` artifacts.
- Location: `scripts/extract_batch_snippets.py`

Usage (dry-run default)
```bash
# show what would be created (no files written)
python3 scripts/extract_batch_snippets.py --batch-path path/to/batch-01-raw.md
```

Write files and backup originals
```bash
# write files beside each input and rename processed input to <name>.bak
python3 scripts/extract_batch_snippets.py --batch-path path/to/batch-01-raw.md --commit
```

Common flags
- `--batch-path <path>`: preferred; accepts a file or a directory of `.md` files
- `--commit`: actually write files and perform backups; default is dry-run
- `--skip-existing`: when used with `--commit` skip creating outputs whose exact `author_year.md` file already exists
- positional `input` argument is still supported for backward compatibility

Behavior notes
- Default output location is the same folder as the input file (no `output/` subfolder).
- Backup behavior: after a successful commit the script renames the input to `<original>.bak` unless the input already ends with `.bak`.
- Filename sanitization: author names are normalized to ASCII and non-alphanumeric characters are replaced with underscores; name collisions are avoided by adding `_a`, `_b`, ... where necessary.
- `.gitignore` in the repo is configured to ignore `docs/**/raw/` contents but allow `.bak` files so backups can be tracked when desired.

Recommended workflow
1. Edit or collect source content into a `batch-xx-raw.md` file in a `docs/.../raw/` folder.
2. Run a dry-run to inspect planned outputs.
3. Run with `--commit --skip-existing` to write only new files.
4. Verify outputs, then add/commit artifacts to the repository as needed.

Support and extension
- If you need additional deduplication or metadata-driven naming rules, update `scripts/extract_batch_snippets.py` and follow the repository contribution conventions.
