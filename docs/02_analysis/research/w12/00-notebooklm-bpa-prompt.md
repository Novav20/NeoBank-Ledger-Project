# PROMPT: NotebookLM BPA Extraction for B2B Ledger

**Context:** I am conducting a formal Business Process Analysis (BPA) for a B2B Neobank Ledger API. This ledger must follow Double-entry accounting principles and ensure high-integrity transactions.

**Instructions:**
Analyze the provided source documents and extract information strictly for the following sections. If a section is not mentioned or information is missing, state "Information not found" for that section. **Do not hallucinate or assume.**

## 1. Context & Scope
- Who are the primary stakeholders (Banks, Businesses, Auditors)?
- What is the stated goal of a Ledger system in these documents?
- What are the boundaries of the system?

## 2. "As-Is" State (Current Processes)
- How are transactions manually recorded or moved between accounts?
- What data is required for a single transaction (Source, Destination, Amount, Timestamp)?
- What is the step-by-step flow of money according to these sources?

## 3. Pain Points (System Entropy)
- What bottlenecks or common errors are mentioned in traditional or legacy systems?
- Are there mentions of "Reconciliation" issues or "Double-spending"?
- Where does the data lose its integrity?

## 4. Business Rules & Logic
- What are the hard rules mentioned (e.g., Total Credits = Total Debits)?
- Are there specific rules for "Pending" vs "Settled" transactions?
- What prevents a transaction from being deleted or modified (Immutability)?

## 5. Metrics & Expected Value
- What are the KPIs mentioned (Transaction speed, Audit time reduction, Accuracy)?

---

**Missing Information Section:**
At the end of your report, list all the critical information from the list above that was **not** found in the sources. Suggest specific technical or financial terms I should search for next to fill these gaps.
