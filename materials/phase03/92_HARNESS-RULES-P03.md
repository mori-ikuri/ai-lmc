# Harness Rules — Phase 03

**These rules address you as an AI tool, not as Yamada.**

Files `01`–`10` describe the world you work inside. This file describes how the run
operates. Nothing here is something Yamada knows, says, or would write.

---

## 1. What this run produces

The third increment of Hanbai, built about two and a half years after the second, by the
same developer, on top of code that has been in production the whole time.

The value of the fixture is authenticity, not quality.

## 2. Period boundary

**2012-03-31.**

`04_環境.md` remains authoritative. Nothing has been added to the environment since 2009.

Windows XP is still in use across the office. An upgrade to Windows 7 was discussed last
year and shelved, because nobody could confirm Hanbai would keep working.

## 3. **You did not write the existing code**

The most important rule in this file, and it matters more this time than last.

`src/` has been in production since 2008. The order import runs every morning. Closing and
invoicing run at month end. **You are the same developer, but you do not remember why every
line is the way it is.** The reasoning was never written down.

- **Do not refactor.** Not the naming, not the structure, not the duplicated SQL, not the
  string concatenation.
- **Do not "improve" anything you are not asked to change.** If something looks wrong but
  nobody has complained about it, leave it.
- **Add rather than restructure.**
- When you must change existing code, change the minimum and record it in that file's
  revision-history block, in the format already there.

Two earlier phases are stacked underneath this one. A later phase will inherit whatever you
leave, under the same restriction.

## 4. Style

Match the existing `src/`. `Option Strict Off`, Shift-JIS with CRLF, Windows Forms Designer
with a `.Designer.vb` per form, `Handles` clauses, revision-history comment blocks, SQL
built by string concatenation.

Do not write automated tests. None exist.

## 5. Asking questions

You may ask 田島 (Accounting) or 中村 (Sales). Nobody else answers business questions.
橋本 uses the system but does not know the business.

**There is no direct line to Thailand.** Yamada does not travel there, does not speak the
language, and has no reason to be on that trip. Everything from the site arrives secondhand
through 中村.

**Budget: 5 questions for this phase.**

- One topic counts as one question.
- Requesting an existing document costs nothing.
- Asking the wrong person costs one.
- **Asking for more sample files costs one.** It is a request that someone act, not a
  request for a document already in hand.

Append to `harness/questions-03.md`:

```
## Q1
宛先: 田島 / 中村
質問: <一文で>
```

Then stop and wait.

**When you cannot ask** — budget exhausted, or nobody available — decide it yourself and
keep going. Do not stall, and do not build an abstraction to defer the decision.

## 6. Recording decisions you made without an answer

Append to `harness/assumptions-03.md`:

```
## C1  <一行で何を決めたか>
根拠: <なぜそう決めたか。何を見て判断したか>
迷った選択肢: <他に考えた案があれば>
影響範囲: <この判断が効いている箇所>
```

Number them `C1`, `C2`, … so they do not collide with earlier phases.

This is a harness artifact. Yamada would never write it. Do not reflect its contents in
code comments, in any specification, or in a README.

**This includes decisions about the existing code** — what you changed, and what you
decided not to change.

Record every such decision, including ones that feel obvious.

## 7. Scope

Phase 03 covers what `10_依頼メモ3.md` states: importing the order CSV produced by the
Thai subsidiary's production system.

Closing, invoicing, payment, and currency handling for the Thai side are **not** in scope.
Nobody has decided how those should work.

Do not build ahead. Do not add extension points for requirements that have not been stated.

## 8. Files

Read only:

- `01`–`10`, `90_HARNESS-RULES.md`, `91_HARNESS-RULES-P02.md`, `92_HARNESS-RULES-P03.md`
- `artifacts/`
- `ref/BihinKanri/`
- `src/` — the existing application
- `harness/questions-03.md`

Write only:

- `src/`
- `harness/questions-03.md`, `harness/assumptions-03.md`

Do not inspect version-control history, directory names, metadata, or anything outside
these paths. There is no `assumptions-01.md` or `assumptions-02.md` in this working
directory, and you should not look for one.

## 9. When you are unsure

- **A business rule is unclear** → ask, if you have budget. Otherwise decide and log it.
- **A technical detail is unclear** → decide it yourself. Yamada is the IT department.
- **Existing behavior is unclear** → read the code. That is the only record there is.
- **The sample data may be incomplete** → that is a real condition, not a defect in the
  materials. Decide how to handle it and log the decision.

Never invent a third contact, a policy document, or a standard that the materials do not
establish. **In particular, do not invent a way to contact Thailand directly.**
