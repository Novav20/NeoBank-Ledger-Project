# Lesson 01: Redactable Ledgers and Chameleon Hashes

## Introduction
In high-integrity systems, we usually want **Immutability** (the inability to change history). However, regulations like **GDPR** (Right to Erasure) and human error (accidental fraud or data entry mistakes) require us to be able to "correct" history.

A **Redactable Ledger** is a system that balances these two. It uses a special type of math called a **Chameleon Hash**.

---

## 1. The Mechatronics Analogy: The "Mechanical Seal"

### Standard Hash (SHA-256)
Think of a **Lead Seal** on a high-precision sensor.
- If you open the sensor to change a component, you **must break the seal**.
- Anyone looking at the sensor can see the seal is broken. History is "Protected" because any change is visible.

### Chameleon Hash
Think of a **Magnetic Lock** with a **Master Key (Trapdoor)**.
- If you don't have the key, it behaves exactly like the lead seal. If you force it, you break it.
- **But**, if you have the Master Key, you can open the lock, swap the component, and close it perfectly.
- To an outside observer, the seal looks **untouched**. The "Fingerprint" of the sensor remains the same.

---

## 2. The Mathematical Engine
A standard hash is a function $H(m)$ where $m$ is the message.
A Chameleon Hash adds a second variable, **Randomness ($r$)**:
$$Hash = CH(m, r)$$

### The "Adapt" Operation
If you possess the **Secret Key (Trapdoor)**, you can perform an **Adaptation**. 
1. You have an original message $m_1$ and its randomness $r_1$.
2. You want to change $m_1$ to $m_2$ (e.g., removing a user's name to comply with GDPR).
3. Using the Secret Key, the algorithm "solves" for a new $r_2$ such that:
   $$CH(m_1, r_1) = CH(m_2, r_2)$$

**Result**: The stored hash value in the block header is identical, so the "Chain" isn't broken, but the content has been changed.

---

## 3. The "Zhao et al. (2024)" Framework: Concordit
The biggest risk of a Chameleon Hash is: **Who has the Master Key?** If one admin has it, they can steal money and hide the evidence.

Zhao's research proposes **Concordit**:
- **Consensus on Redaction**: The Master Key is split among a committee (using Threshold Cryptography). 
- **Credit-Based Rewards**: Nodes get "Credit" for participating in honest redactions. If a node tries to redact history maliciously, their credit is destroyed, and they are kicked out of the network.
- **Accountability**: Every time the "Master Key" is used, an immutable **Audit Log** records *who* requested the change and *why*.

---

## 4. Why this matters for ADR-001
In our Ledger:
- We will store the **Chameleon Hash** in our Block structure.
- We will define a **Governance Rule** (based on GDPR Article 17) for when that key can be used.
- We will ensure that while the *data* changes, the *fact that a change happened* is permanently recorded in a separate audit trail.

---

## Technical Summary for Engineers
| Component | Function in our Project |
| :--- | :--- |
| **Message ($m$)** | The transaction data (PII, amounts). |
| **Randomness ($r$)** | The "Modifier" that lets us keep the hash stable. |
| **Trapdoor Key** | Held by the "Compliance Committee" (Sponsor Bank + Fintech). |
| **Redaction Trigger** | A valid GDPR request or a court order. |
