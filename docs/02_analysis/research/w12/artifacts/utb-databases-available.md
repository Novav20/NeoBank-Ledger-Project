| Database                       | Description                                                                                                                               |
| ------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------- |
| Digitalia Film Library         | Streaming collection of films, arthouse, and documentaries (≈1,827 titles).                                                               |
| Digitalia Hispánica            | Multidisciplinary Spanish-language ebook collection (architecture, journalism, social sciences, economics, law, education, engineering).  |
| Ebooks724                      | Ebook platform supporting basic sciences and engineering programs.                                                                        |
| EBSCO eBooks                   | Full-text ebook collection (including project management and related business topics).                                                    |
| Emerald Insight                | Collection including Emerging Markets case studies, The Case Journal, and thematic collections such as The Case for Women.                |
| Gale eBooks (GVRL)             | Ebooks focused on communication, project management, and related social sciences.                                                         |
| Gestor (GestorCC)              | Colombian corporate information system — financial statements, P&L and financial indicators for ~45,000 companies across sectors.         |
| JSTOR                          | Full-text scholarly content in economics and related humanities/social sciences.                                                          |
| Legis (Human Resources portal) | HR-focused portal with resources on HR management, case studies, competency frameworks, regulations and learning events.                  |
| Legis-Comex                    | Trade and customs portal with tools and reference material for export/import processes.                                                   |
| Nature                         | Interdisciplinary scientific journal collection with peer-reviewed research across science, technology and medicine.                      |
| Oxford Academic (Journals)     | Collection of scholarly journals across life sciences, social sciences, mathematics, law and medicine (includes many Open Access titles). |
| SAGE Collections               | Large collections of books and journals in business, humanities, social sciences, science and medicine (multiple SAGE platforms listed).  |
| ScienceDirect                  | Elsevier's full-text database of scientific journals across science, technology and medicine.                                             |
| ScienceDirect Topics           | Curated topic pages and concept summaries to aid understanding of technical terms and concepts.                                           |
| Scopus                         | Bibliographic and citation database with bibliometrics tools; largest abstract/index of peer-reviewed literature.                         |
| SpringerLink                   | Multidisciplinary journals and ebooks (large journal and ebook collections, publisher collections included).                              |
| Taylor & Francis / Routledge   | Multidisciplinary journal collection and ebook access, with strong coverage in social sciences and engineering.                           |
| vLex Colombia                  | Legal research platform with Colombian legislation, regulations, treaties, and legal commentary.                                          |

---

## Strategic Research Prioritization (Batch 01)

### Top 5 Recommended Databases
1. **ScienceDirect**: Primary source for peer-reviewed papers on Software Architecture, Distributed Systems, and Database Integrity.
    *   *Search Terms*: "distributed ledger architecture", "ACID compliance financial systems", "API-first banking design", "immutable database patterns".
2. **Emerald Insight**: Best for Business Case Studies on Fintech models and B2B payment workflows.
    *   *Search Terms*: "B2B Neobanking case study", "Banking-as-a-Service ecosystem", "embedded finance B2B workflows", "fintech platform orchestration".
3. **JSTOR**: Ideal for fundamental Economic and Accounting theory (Double-entry principles).
    *   *Search Terms*: "history of double-entry bookkeeping", "accounting internal controls", "financial auditability standards", "theory of the firm banking".
4. **Scopus**: The "Search Hub" to identify highly-cited research across all engineering/finance domains.
    *   *Search Terms*: "Fintech API security", "ledger integrity verification", "real-time settlement systems", "ISO 20022 implementation challenges".
5. **vLex Colombia**: Essential for local (Colombian) Regulatory Context and Fintech law.
    *   *Search Terms*: "Ley Fintech Colombia", "Open Banking SFC", "Cuentas de ahorro de trámite simplificado", "Protección de datos financieros".

### Architect's Strategy
Start with **Scopus** to identify ~10 core papers from 2023-2025. Then, use **ScienceDirect** and **Emerald Insight** to retrieve full-text PDFs for the first NotebookLM ingestion.

### Most Important Source Types for This Research:
1. Process Flow & Case Studies: (Emerald Insight / ScienceDirect) - Papers describing how Banking-as-a-Service (BaaS) platforms orchestrate funds between a Fintech and a Sponsor Bank.
   2. Data Integrity & Architecture Whitepapers: (ScienceDirect / Scopus) - Research on ACID compliance, Event Sourcing or Immutable Ledger databases.
   3. Regulatory & Standards Frameworks: (vLex / Emerald) - Information on ISO 20022 (the global standard for financial messaging) and local Colombian banking regulations.
   4. Accounting First Principles: (JSTOR) - Peer-reviewed deep dives into the logic of Double-entry bookkeeping to ensure the "Plant" (the Ledger) is always balanced.
### Source Filtering Guidance (Avoiding System Noise)
- **Prioritize**: B2B Payment Rails, Reconciliation Processes, BaaS Governance, Ledger Interoperability, and 'Technical Plumbing'.
- **Avoid**: Marketing, Consumer Behavior, UI/UX, and AI Chatbots (these are 'System Noise' for Backend Logic).
- **High-Signal Tip**: Look for chapters titled 'The Plumbing of Fintech' or similar in Handbooks (e.g., Emerald Handbook of Fintech).
