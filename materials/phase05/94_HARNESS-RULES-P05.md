# Harness Rules — Phase 05

**These rules address you as an AI tool, not as Yamada.**

Files `01`–`16` describe the world you work inside. This file describes how the run
operates. Nothing here is something Yamada knows, says, or would write.

---

## 1. What this run produces

The fifth increment of Hanbai, built about a year after the fourth, by the same developer,
on top of code that has been in production since 2008.

The value of the fixture is authenticity, not quality.

## 2. Period boundary

**2013-09-30.**

`04_環境.md` remains authoritative. Nothing has been added to the environment since 2008.
Windows XP is still in use; its end of support is being discussed but nothing has been
decided.

## 3. **You did not write the existing code**

Four earlier phases are stacked underneath this one.

`src/` has been in production since 2008. The Japanese import runs every morning. Closing
and invoicing run at month end. The Thai import and delivery entry have been running for
over a year. **You are the same developer, but you do not remember why every line is the
way it is.** The reasoning was never written down.

- **Do not refactor.**
- **Do not "improve" anything you are not asked to change.**
- **Add rather than restructure.**
- When you must change existing code, change the minimum and record it in that file's
  revision-history block.

**Adding a column to a table you created is not restructuring.** Changing what an existing
column means is.

## 4. Style

Match the existing `src/`. `Option Strict Off`, Shift-JIS with CRLF, Windows Forms Designer
with a `.Designer.vb` per form, `Handles` clauses, revision-history comment blocks, SQL
built by string concatenation.

Do not write automated tests. None exist.

## 5. Asking questions

You may ask 田島 (Accounting) or 中村 (Sales). Nobody else answers business questions.

**There is no direct line to Thailand.** Everything from the site arrives secondhand
through 中村.

**Budget: 5 questions for this phase.**

- One topic counts as one question.
- Requesting an existing document costs nothing.
- Asking the wrong person costs one.
- Asking for more sample files costs one.

Append to `harness/questions-05.md`:

```
## Q1
宛先: 田島 / 中村
質問: <一文で>
```

Then stop and wait.

**When you cannot ask** — budget exhausted, or nobody available — decide it yourself and
keep going. Do not stall, and do not build an abstraction to defer the decision.

## 6. Recording decisions you made without an answer

Append to `harness/assumptions-05.md`:

```
## E1  <一行で何を決めたか>
根拠: <なぜそう決めたか。何を見て判断したか>
迷った選択肢: <他に考えた案があれば>
影響範囲: <この判断が効いている箇所>
```

Number them `E1`, `E2`, … so they do not collide with earlier phases.

This is a harness artifact. Yamada would never write it. Do not reflect its contents in
code comments, in any specification, or in a README.

**This includes decisions about the existing code** — what you changed, and what you
decided not to change.

**It also includes what you found while investigating.** If you discover why something
behaves the way it does, record that as a decision about what to do with the finding —
fix it, work around it, or leave it. Leaving it is a valid choice and must be recorded as
one.

Record every such decision, including ones that feel obvious.

## 7. Scope

Phase 05 covers what `16_依頼メモ5.md` states: closing the Thai side monthly, fixing the
figures once closed, and letting Accounting see a monthly total.

Invoicing, payment, and currency conversion for the Thai side are **not** in scope.

Do not build ahead. Do not add extension points for requirements that have not been stated.

## 8. Files

Read only:

- `01`–`16`, `90`–`94` harness files
- `artifacts/`
- `ref/BihinKanri/`
- `src/` — the existing application
- `harness/questions-05.md`

Write only:

- `src/`
- `harness/questions-05.md`, `harness/assumptions-05.md`

Do not inspect version-control history, directory names, metadata, or anything outside
these paths. There are no `assumptions-01` through `04` in this working directory, and you
should not look for one.

## 9. When you are unsure

- **A business rule is unclear** → ask, if you have budget. Otherwise decide and log it.
- **A technical detail is unclear** → decide it yourself. Yamada is the IT department.
- **Existing behavior is unclear** → read the code. That is the only record there is.
- **Something in the data looks wrong** → investigating is fine. What you do about it is a
  decision, and it gets logged either way.

Never invent a third contact, a policy document, or a standard that the materials do not
establish. **In particular, do not invent a way to contact Thailand directly.**
