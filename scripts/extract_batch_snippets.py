#!/usr/bin/env python3
"""
extract_batch_snippets.py

Read a markdown file containing multiple triple-backticked code blocks (each containing
YAML-like frontmatter and content) and write each block into a separate .md file.

Usage:
  python3 scripts/extract_batch_snippets.py input.md output_dir

Behavior:
- Finds all ```...``` fenced blocks and extracts the inner text.
- Parses a YAML-style frontmatter block (--- ... ---) to obtain `main_author` and `year`.
- Builds a sanitized filename from the first-listed author and year: mainauthor_YYYY.md
- Avoids overwriting by adding a, b, c suffix when necessary: mainauthor_YYYY_a.md
- Replaces unusual characters in author names with safe ASCII characters per repo
  naming rules.
"""

from __future__ import annotations

import argparse
import re
import sys
import unicodedata
from pathlib import Path
from typing import List, Optional, Tuple


def slugify(value: str) -> str:
    """Return a filesystem-safe slug for `value`.

    Normalizes unicode, removes non-alphanumeric characters and collapses
    runs of non-alphanumerics into single underscores. Lowercases the result.
    """
    if not value:
        return "unknown"
    value = unicodedata.normalize("NFKD", value)
    value = value.encode("ascii", "ignore").decode("ascii")
    value = re.sub(r"[^A-Za-z0-9]+", "_", value)
    value = re.sub(r"_+", "_", value)
    value = value.strip("_")
    return value.lower() or "unknown"


def find_fenced_blocks(text: str) -> List[str]:
    # Matches ``` optionally followed by a language, then newline, then content, until closing ```
    pattern = re.compile(r"```(?:[a-zA-Z0-9_-]+)?\n(.*?)\n```", re.DOTALL)
    return pattern.findall(text)


def parse_frontmatter(block: str) -> Tuple[Optional[str], Optional[str]]:
    """Extract main_author and year from a YAML-like frontmatter inside the block.

    Returns (main_author, year) or (None, None) if not found.
    """
    fm_match = re.search(r"---\s*(.*?)\s*---", block, re.DOTALL)
    if not fm_match:
        return None, None
    fm = fm_match.group(1)
    ma = None
    yr = None
    for line in fm.splitlines():
        line = line.strip()
        if line.lower().startswith("main_author:"):
            _, val = line.split(":", 1)
            val = val.strip()
            if (val.startswith('"') and val.endswith('"')) or (val.startswith("'") and val.endswith("'")):
                val = val[1:-1]
            ma = val
        elif line.lower().startswith("year:"):
            _, val = line.split(":", 1)
            val = val.strip()
            y_match = re.search(r"(20\d{2}|19\d{2})", val)
            if y_match:
                yr = y_match.group(1)
    return ma, yr


def choose_author_label(main_author_field: Optional[str]) -> str:
    """From the `main_author` field pick a concise label to use in filenames.

    Strategy: take the first listed author (split on comma or ' and '), then
    use the last token as surname if it looks like a full name; otherwise use the
    whole token. Finally slugify the result.
    """
    if not main_author_field:
        return "unknown"
    first = re.split(r",| and | & |\\band\\b", main_author_field, maxsplit=1)[0].strip()
    parts = first.split()
    candidate = parts[-1] if len(parts) > 1 else first
    return slugify(candidate)


def unique_filename(directory: Path, base: str, ext: str = ".md") -> Path:
    """Return a Path inside `directory` that does not overwrite existing files.

    If base.ext exists, append _a, _b, ... before the extension.
    """
    directory.mkdir(parents=True, exist_ok=True)
    candidate = directory / f"{base}{ext}"
    if not candidate.exists():
        return candidate
    for i in range(1, 100):
        suffix = chr(ord("a") + (i - 1))
        candidate = directory / f"{base}_{suffix}{ext}"
        if not candidate.exists():
            return candidate
    i = 1
    while True:
        candidate = directory / f"{base}_{i}{ext}"
        if not candidate.exists():
            return candidate
        i += 1


def extract_and_write(input_path: Path, output_dir: Path, commit: bool = False, skip_existing: bool = False) -> List[Path]:
    """Extract fenced blocks from `input_path` and optionally write them to `output_dir`.

    If `commit` is False the function performs a dry-run: it calculates the files
    that would be created and prints them, but does not write or rename files.
    """
    text = input_path.read_text(encoding="utf-8")
    blocks = find_fenced_blocks(text)
    created: List[Path] = []
    seen_bases = set()
    for block in blocks:
        main_author_field, year = parse_frontmatter(block)
        author_label = choose_author_label(main_author_field)
        year_label = year if year else "xxxx"
        base = f"{author_label}_{year_label}"

        # Candidate path without suffix
        candidate = output_dir / f"{base}.md"

        if candidate.exists():
            if skip_existing:
                print(f"Skipping existing: {candidate}")
                continue
            # else fall through and generate a unique filename (adds _a, _b...)

        # If candidate does not exist, use it; otherwise find a unique filename
        if not candidate.exists():
            out_path = candidate
        else:
            out_path = unique_filename(output_dir, base, ext=".md")

        created.append(out_path)
        seen_bases.add(base)
        if commit:
            # Write the block content (trim leading/trailing blank lines)
            content = block.strip("\n") + "\n"
            out_path.write_text(content, encoding="utf-8")
            print(f"Wrote: {out_path}")
        else:
            print(f"Would create: {out_path}")
    return created


def main(argv: Optional[List[str]] = None) -> int:
    import argparse

    p = argparse.ArgumentParser(description="Extract fenced blocks into individual markdown files")
    p.add_argument("input", nargs="?", default=None, help="(Deprecated) Path to input markdown file or directory. Use --batch-path instead.")
    p.add_argument("--batch-path", dest="batch_path", help="Path to input markdown file or directory (file or folder). Preferred over positional 'input'.")
    p.add_argument("output_dir", nargs="?", help="Directory to write output files (default: ./output relative to each input)", default=None)
    p.add_argument("--commit", action="store_true", help="Actually write files and backup originals. Default: dry-run (no changes).")
    p.add_argument("--skip-existing", action="store_true", help="When committing, skip creating files whose exact author_year.md already exists (prevents duplicates).")
    args = p.parse_args(argv)

    # Prefer --batch-path if provided, otherwise fall back to positional `input` for backward compatibility
    chosen = args.batch_path if args.batch_path else args.input
    if not chosen:
        print("Error: no input provided. Pass --batch-path <path> or the positional input.", file=sys.stderr)
        return 2
    input_path = Path(chosen)
    if not input_path.exists():
        print(f"Input path not found: {input_path}", file=sys.stderr)
        return 2

    # Collect input files: single file or all .md files in a directory (non-recursive)
    inputs: List[Path] = []
    if input_path.is_dir():
        for pth in sorted(input_path.iterdir()):
            if pth.is_file() and pth.suffix.lower() == ".md" and not pth.name.endswith(".bak"):
                inputs.append(pth)
    else:
        inputs.append(input_path)

    total_planned = 0
    total_written = 0
    for inp in inputs:
        # Determine output directory for this input
        if args.output_dir:
            outdir = Path(args.output_dir)
        else:
            # Default: write output files next to the original input file
            outdir = inp.parent
        planned = extract_and_write(inp, outdir, commit=args.commit, skip_existing=args.skip_existing)
        total_planned += len(planned)
        if args.commit:
            # If commit was requested and at least one file written, backup original
            if planned:
                try:
                    # If the input already ends with .bak, do not append another .bak
                    if inp.name.endswith('.bak'):
                        print(f"Input {inp} already has .bak suffix; leaving original in place.")
                    else:
                        bak_path = inp.with_name(inp.name + ".bak")
                        if bak_path.exists():
                            print(f"Backup already exists: {bak_path}; leaving original in place.", file=sys.stderr)
                        else:
                            inp.rename(bak_path)
                            print(f"Backed up original to {bak_path}")
                except Exception as e:
                    print(f"Warning: could not rename original file to {bak_path}: {e}", file=sys.stderr)
            total_written += len(planned)

    if args.commit:
        print(f"Wrote {total_written} files from {len(inputs)} input(s)")
    else:
        print(f"Dry-run: {total_planned} files would be created from {len(inputs)} input(s). Use --commit to apply changes.")

    return 0


if __name__ == "__main__":
    raise SystemExit(main())
