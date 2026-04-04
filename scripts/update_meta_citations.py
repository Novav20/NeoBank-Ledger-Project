#!/usr/bin/env python3
"""
update_meta_citations.py

Reads a meta-analysis source document to see which research sources are cited.
Then updates the YAML frontmatter of the batch source files to include:
  cited_in_meta_analysis: true|false

Usage:
  python3 scripts/update_meta_citations.py --meta-report path/to/META_Report.md --sources-dir path/to/batch-02 [--commit]

Behavior:
- Reads the meta-analysis report text.
- Iterates over all .md files in the sources directory (ignoring .bak files).
- Checks if the stem (filename without .md) is present in the report text.
- If present, sets `cited_in_meta_analysis: true`; if not, `false`.
- If --commit is omitted, it performs a dry-run.
"""

import argparse
import re
import sys
from pathlib import Path


def update_frontmatter(content: str, cited: bool) -> str:
    """Updates or adds cited_in_meta_analysis to the YAML frontmatter."""
    parts = content.split('---', 2)
    if len(parts) >= 3:
        frontmatter = parts[1]

        if 'cited_in_meta_analysis:' in frontmatter:
            new_frontmatter = re.sub(
                r'cited_in_meta_analysis:\s*(true|false)',
                f'cited_in_meta_analysis: {str(cited).lower()}',
                frontmatter
            )
        else:
            new_frontmatter = frontmatter.rstrip() + f'\ncited_in_meta_analysis: {str(cited).lower()}\n'

        return parts[0] + '---' + new_frontmatter + '---' + parts[2]
    return content


def main():
    parser = argparse.ArgumentParser(description="Update batch sources with cited_in_meta_analysis status.")
    parser.add_argument("--meta-report", required=True, help="Path to the meta-analysis report file")
    parser.add_argument("--sources-dir", required=True, help="Directory containing the research source .md files")
    parser.add_argument("--commit", action="store_true", help="Apply changes. Default is dry-run.")
    args = parser.parse_args()

    meta_path = Path(args.meta_report)
    sources_dir = Path(args.sources_dir)

    if not meta_path.exists():
        print(f"Error: meta report not found at {meta_path}")
        return 1

    if not sources_dir.exists() or not sources_dir.is_dir():
        print(f"Error: sources directory not found at {sources_dir}")
        return 1

    report_content = meta_path.read_text(encoding="utf-8")

    modified_count = 0
    cited_count = 0
    not_cited_count = 0
    total_sources = 0

    for source_file in sorted(sources_dir.glob("*.md")):
        if source_file.name.endswith(".bak"):
            continue

        total_sources += 1
        stem = source_file.stem

        is_cited = stem in report_content

        if is_cited:
            cited_count += 1
        else:
            not_cited_count += 1

        content = source_file.read_text(encoding="utf-8")
        new_content = update_frontmatter(content, is_cited)

        if new_content != content:
            if args.commit:
                source_file.write_text(new_content, encoding="utf-8")
                print(f"Updated {source_file.name}: cited_in_meta_analysis={str(is_cited).lower()}")
            else:
                print(f"Would update {source_file.name}: cited_in_meta_analysis={str(is_cited).lower()}")
            modified_count += 1

    print("-" * 40)
    print(f"Total sources processed: {total_sources}")
    print(f"Sources cited in meta-analysis: {cited_count}")
    print(f"Sources NOT cited in meta-analysis: {not_cited_count}")
    if args.commit:
        print(f"Files actually modified: {modified_count}")
    else:
        print(f"Dry-run: {modified_count} files would be modified. Use --commit to apply changes.")

if __name__ == "__main__":
    sys.exit(main())
