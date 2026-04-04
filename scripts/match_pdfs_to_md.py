#!/usr/bin/env python3
"""Match PDF filenames against generated markdown titles using difflib.

Usage: python3 scripts/match_pdfs_to_md.py --pdf-dir <pdf_dir> --md-dir <md_dir> [--threshold 0.65]
"""
import argparse
import difflib
import os
import re
import sys


def norm(s: str) -> str:
    s = s or ""
    s = s.lower()
    s = re.sub(r"[^a-z0-9]+", " ", s)
    s = re.sub(r"\s+", " ", s).strip()
    return s


def extract_title_from_md(path: str) -> str:
    try:
        with open(path, "r", encoding="utf-8") as f:
            text = f.read()
    except Exception:
        return ""
    # find YAML frontmatter between leading '---' blocks
    m = re.match(r"^---\s*(.*?)---\s", text, re.S)
    if not m:
        # try to find first title: line anywhere
        m2 = re.search(r"^title:\s*(?:\"([^\"]+)\"|'([^']+)'|(.+))$", text, re.M)
        if m2:
            return (m2.group(1) or m2.group(2) or m2.group(3) or "").strip()
        return ""
    fm = m.group(1)
    # search for title: in frontmatter
    m3 = re.search(r"^title:\s*(?:\"([^\"]+)\"|'([^']+)'|(.+))$", fm, re.M)
    if m3:
        return (m3.group(1) or m3.group(2) or m3.group(3) or "").strip()
    return ""


def extract_titles_from_raw_md(path: str):
    try:
        with open(path, "r", encoding="utf-8") as f:
            text = f.read()
    except Exception:
        return []
    # find all YAML frontmatter blocks
    blocks = re.findall(r"^---\s*(.*?)\s*---", text, re.S | re.M)
    entries = []
    for i, fm in enumerate(blocks):
        m3 = re.search(r"^title:\s*(?:\"([^\"]+)\"|'([^']+)'|(.+))$", fm, re.M)
        title = (m3.group(1) or m3.group(2) or m3.group(3) or "").strip() if m3 else ""
        entries.append((f"{os.path.relpath(path)}::block{i+1}", title, norm(title)))
    return entries


def main():
    p = argparse.ArgumentParser()
    p.add_argument("--pdf-dir", required=True)
    p.add_argument("--md-dir", required=False)
    p.add_argument("--raw-md", required=False, help="Path to the raw batch markdown to extract multiple titles from")
    p.add_argument("--threshold", type=float, default=0.65)
    args = p.parse_args()

    pdf_dir = args.pdf_dir
    md_dir = args.md_dir
    thresh = args.threshold

    if not os.path.isdir(pdf_dir):
        print(f"PDF dir not found: {pdf_dir}")
        sys.exit(2)
    if args.raw_md:
        if not os.path.isfile(args.raw_md):
            print(f"Raw MD file not found: {args.raw_md}")
            sys.exit(2)
    else:
        if not md_dir or not os.path.isdir(md_dir):
            print(f"MD dir not found: {md_dir}. Or provide --raw-md instead.")
            sys.exit(2)

    pdfs = []
    for fn in sorted(os.listdir(pdf_dir)):
        if fn.lower().endswith('.pdf'):
            name = os.path.splitext(fn)[0]
            pdfs.append((fn, norm(name)))

    mds = []
    if args.raw_md:
        mds = extract_titles_from_raw_md(args.raw_md)
    else:
        for root, _, files in os.walk(md_dir):
            for fn in sorted(files):
                if fn.lower().endswith('.md'):
                    path = os.path.join(root, fn)
                    title = extract_title_from_md(path)
                    mds.append((os.path.relpath(path), title, norm(title)))

    # Build list of md normalized titles
    md_norms = [md_norm for (_, _, md_norm) in mds]

    matches = []
    unmatched_pdfs = []
    for pdf_fn, pdf_norm in pdfs:
        best_ratio = 0.0
        best_idx = None
        for i, md_norm in enumerate(md_norms):
            if not md_norm:
                continue
            r = difflib.SequenceMatcher(None, pdf_norm, md_norm).ratio()
            if r > best_ratio:
                best_ratio = r
                best_idx = i
        if best_ratio >= thresh and best_idx is not None:
            matches.append((pdf_fn, mds[best_idx][0], best_ratio))
        else:
            unmatched_pdfs.append((pdf_fn, pdf_norm, best_ratio, mds[best_idx][0] if best_idx is not None else None))

    # Print results
    print("MATCHED (pdf -> md) with ratio:\n")
    for pdf_fn, md_path, ratio in matches:
        print(f"{pdf_fn} -> {md_path}  (ratio={ratio:.2f})")

    print("\nUNMATCHED PDFs (best ratio < threshold):\n")
    for pdf_fn, pdf_norm, best_ratio, best_md in unmatched_pdfs:
        print(f"{pdf_fn}  best_ratio={best_ratio:.2f}  best_md={best_md}")

    print(f"\nSummary: {len(pdfs)} PDFs, {len(mds)} MD files, matched={len(matches)}, unmatched={len(unmatched_pdfs)}")


if __name__ == '__main__':
    main()
