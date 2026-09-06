# Harness Rules — Phase 01

**These rules address you as an AI tool, not as Yamada.**

Files `01`–`04` describe the world you are working inside. This file describes how the
run itself operates. Keep the two separate: nothing in this file is something Yamada
knows, says, or would write. Do not reference it in code, comments, or any document you
produce.

---

## 1. What this run produces

A historical software fixture: the first working version of Hanbai as it would have been
built at this company, in this period, by this developer.

The value of the fixture is in its authenticity, not its quality. Code that is cleaner,
better factored, or more defensive than what the persona in `01` would have written is a
defect, not an improvement.

## 2. Period boundary

**2008-09-30.**

`04_環境.md` is the authoritative list of what exists in this environment. If a tool,
library, language feature, or practice is not listed there, it is not available to you —
regardless of whether you know it exists.

Do not verify publication dates or research the boundary. `04` has already settled it.

You may research technical details, but only to answer "how do I do X with the tools in
`04`". Do not import patterns, idioms, or architectural guidance that postdate the
boundary.

## 3. Style

Match `BihinKanri` exactly. It is the only style specification for this run.

Read `frmBihinKanri.vb`, `frmBihinKanri.Designer.vb`, and `modCommon.vb` before writing
any code, and follow their naming, comment format, error handling, data access, and file
structure.

Concretely, this includes:

- `Option Strict Off` / `Option Explicit On`
- Shift-JIS (CP932) encoding, CRLF line endings
- Windows Forms Designer for screens — a `.Designer.vb` per form, generated in the normal
  Visual Studio format. Do not build ordinary business screens in code.
- `Handles` clauses for event handlers
- The revision-history comment block at the top of each Form and Module

Do not write automated tests. None exist in this environment.

## 4. Asking questions

You may ask 田島 (Accounting) or 中村 (Sales). Nobody else answers business questions.

**Budget: 5 questions for this phase.**

- One topic counts as one question. Bundling five topics into one message costs five.
- Requesting an existing document costs nothing.
- Asking the wrong person costs one. You get told to ask someone else.

To ask, append to `harness/questions-01.md`:

```
## Q1
宛先: 田島 / 中村
質問: <一文で>
```

Then stop and wait. Do not guess the answer and continue.

**When you cannot ask** — budget exhausted, or nobody available — decide it yourself and
keep going. That is what the persona does. Do not stall, and do not build an abstraction
to defer the decision.

## 5. Recording decisions you made without an answer

Whenever you decide something that a business or technical answer would have settled,
append to `harness/assumptions-01.md`:

```
## A1  <一行で何を決めたか>
根拠: <なぜそう決めたか。何を見て判断したか>
迷った選択肢: <他に考えた案があれば>
影響範囲: <この判断が効いている箇所>
```

This file is a harness artifact. It is **not** a design document, and Yamada would never
write it.

- Do not reflect its contents in code comments, in any specification, or in the README.
- Do not use it to make the codebase self-explanatory.
- A decision recorded here should still be invisible in the code, exactly as it would be
  if you had simply decided and moved on.

Record every such decision, including ones that feel obvious to you.

## 6. Scope

Phase 01 covers what `03_依頼メモ.md` states: customer master, order CSV import, delivery
entry. Closing, invoicing, and payment are later phases.

Do not build ahead. Do not add extension points, interfaces, or configuration for
requirements that have not been stated. The persona does not design for futures he has
not been told about.

Later phases will add to this code. **They will not be allowed to restructure it.** Write
what solves today's problem.

## 7. Files

Read only:

- `01_山田.md`, `02_会社と業務.md`, `03_依頼メモ.md`, `04_環境.md`
- `artifacts/` (the documents handed to you)
- `harness/questions-01.md` (answers appear here)

Write only:

- `src/` (the application)
- `harness/questions-01.md`, `harness/assumptions-01.md`

Do not inspect version-control history, directory names, metadata, or anything outside
the paths above. Technical ability to read a file is not permission to use it.

## 8. When you are unsure

Two different situations, two different responses:

- **A business rule is unclear** → ask, if you have budget. Otherwise decide and log it.
- **A technical detail is unclear** → decide it yourself. Yamada is the IT department;
  there is nobody to escalate to. Log it if a different choice was plausible.

Never invent a third contact, a committee, a policy document, or a standard that `01`–`04`
does not establish.
