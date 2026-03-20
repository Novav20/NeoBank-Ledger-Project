#!/usr/bin/env python3
"""
update_bpa_citations.py

Reads the BPA_Report.md to see which research sources are cited.
Then updates the YAML frontmatter of the batch source files to include:
cited_in_bpa: true|false

Usage:
  python3 scripts/update_bpa_citations.py --bpa-report path/to/BPA_Report.md --sources-dir path/to/batch-01 [--commit]

Behavior:
- Reads the BPA report text.
- Iterates over all .md files in the sources directory (ignoring .bak files).
- Checks if the stem (filename without .md) is present in the BPA report text. (e.g., [[alfhaili_2025|...]])
- If present, sets `cited_in_bpa: true` in the YAML frontmatter; if not, `cited_in_bpa: false`.
- If --commit is omitted, it performs a dry run and prints statistics without making changes.
"""

import argparse
import re
import sys
from pathlib import Path

def update_frontmatter(content: str, cited: bool) -> str:
    """Updates or adds cited_in_bpa to the YAML frontmatter."""
    parts = content.split('---', 2)
    if len(parts) >= 3:
        frontmatter = parts[1]
        
        # Check if cited_in_bpa exists
        if 'cited_in_bpa:' in frontmatter:
            # Replace existing
            new_frontmatter = re.sub(
                r'cited_in_bpa:\s*(true|false)', 
                f'cited_in_bpa: {str(cited).lower()}', 
                frontmatter
            )
        else:
            # Append it
            new_frontmatter = frontmatter.rstrip() + f'\ncited_in_bpa: {str(cited).lower()}\n'
        
        return parts[0] + '---' + new_frontmatter + '---' + parts[2]
    return content

def main():
    parser = argparse.ArgumentParser(description="Update batch sources with cited_in_bpa status.")
    parser.add_argument("--bpa-report", required=True, help="Path to the BPA_Report.md")
    parser.add_argument("--sources-dir", required=True, help="Directory containing the research source `.md` files")
    parser.add_argument("--commit", action="store_true", help="Apply changes. Default is dry-run.")
    
    args = parser.parse_args()
    
    bpa_path = Path(args.bpa_report)
    sources_dir = Path(args.sources_dir)
    
    if not bpa_path.exists():
        print(f"Error: BPA report not found at {bpa_path}")
        return 1
    
    if not sources_dir.exists() or not sources_dir.is_dir():
        print(f"Error: Sources directory not found at {sources_dir}")
        return 1
        
    bpa_content = bpa_path.read_text(encoding="utf-8")
    
    modified_count = 0
    cited_count = 0
    not_cited_count = 0
    total_sources = 0
    
    # Iterate over markdown files
    for source_file in sources_dir.glob("*.md"):
        if source_file.name.endswith(".bak"):
            continue
            
        total_sources += 1
        stem = source_file.stem  # e.g., alt_2025
        
        # Check if the stem is cited in the BPA report
        # We look for the exact stem name, which should be in the citing wikilink e.g., [[alt_2025|...]]
        is_cited = stem in bpa_content
        
        if is_cited:
            cited_count += 1
        else:
            not_cited_count += 1
            
        content = source_file.read_text(encoding="utf-8")
        new_content = update_frontmatter(content, is_cited)
        
        if new_content != content:
            if args.commit:
                source_file.write_text(new_content, encoding="utf-8")
                print(f"Updated {source_file.name}: cited_in_bpa={str(is_cited).lower()}")
            else:
                print(f"Would update {source_file.name}: cited_in_bpa={str(is_cited).lower()}")
            modified_count += 1
        else:
            # Already correct (e.g., ran --commit twice)
            pass
            
    print("-" * 40)
    print(f"Total sources processed: {total_sources}")
    print(f"Sources cited in BPA: {cited_count}")
    print(f"Sources NOT cited in BPA: {not_cited_count}")
    
    if args.commit:
        print(f"Files actually modified: {modified_count}")
    else:
        print(f"Dry-run: {modified_count} files would be modified. Use --commit to apply changes.")

if __name__ == "__main__":
    sys.exit(main())
