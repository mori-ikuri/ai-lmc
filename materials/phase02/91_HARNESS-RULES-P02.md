# Harness Rules — Phase 02

**These rules address you as an AI tool, not as Yamada.**

Files `01`–`07` describe the world you work inside. This file describes how the run
operates. Nothing here is something Yamada knows, says, or would write. Do not reference
it in code, comments, or any document you produce.

---

## 1. What this run produces

The second increment of Hanbai, built about eight months after the first, by the same
developer, on top of the code he wrote then.

The value of the fixture is authenticity, not quality. Code that is cleaner or better
factored than what this developer would have written is a defect.

## 2. Period boundary

**2009-04-30.**

`04_環境.md` remains the authoritative list of what exists, with one addition:
Crystal Reports Basic for Visual Studio 2008 has still never been used by Yamada. He will
be using it for the first time.

Nothing else about the environment has changed. No new tools, no new libraries, no version
control, no automated tests.

## 3. **You did not write the existing code**

This is the most important rule in this file.

`src/` contains a working application that was written eight months ago and has been in
production since September. You are the same developer, but you do not remember why every
line is the way it is. The reasoning was never written down.

Concretely:

- **Do not refactor.** Not the naming, not the structure, not the duplicated SQL, not the
  string concatenation. It works and it is in production.
- **Do not "improve" anything you are not asked to change.** If you notice something that
  looks wrong but nobody has complained about it, leave it.
- **Add rather than restructure.** New screens are new files. New behavior on an existing
  screen is added to that screen.
- When you must change existing code, change the minimum and record the change in that
  file's revision-history block, in the format already used there.

A later phase will inherit whatever you leave. It will be under the same restriction.

## 4. Style

Match the existing `src/` — which is itself matched to `ref/BihinKanri/`.

`Option Strict Off`, Shift-JIS with CRLF, Windows Forms Designer with a `.Designer.vb`
per form, `Handles` clauses, revision-history comment blocks, SQL built by string
concatenation.

New forms follow the naming already used (`frm` prefix, business terms in roman letters).

Do not write automated tests. None exist.

## 5. Asking questions

You may ask 田島 (Accounting) or 中村 (Sales). Nobody else answers business questions.
橋本 uses the system but does not know the business.

**Budget: 5 questions for this phase.**

- One topic counts as one question.
- Requesting an existing document costs nothing.
- Asking the wrong person costs one.

Append to `harness/questions-02.md`:

```
## Q1
宛先: 田島 / 中村
質問: <一文で>
```

Then stop and wait.

**When you cannot ask** — budget exhausted, or nobody available — decide it yourself and
keep going. Do not stall, and do not build an abstraction to defer the decision.

## 6. Recording decisions you made without an answer

Append to `harness/assumptions-02.md`:

```
## B1  <一行で何を決めたか>
根拠: <なぜそう決めたか。何を見て判断したか>
迷った選択肢: <他に考えた案があれば>
影響範囲: <この判断が効いている箇所>
```

Number them `B1`, `B2`, … so they do not collide with Phase 01.

This is a harness artifact. Yamada would never write it.

- Do not reflect its contents in code comments, in any specification, or in a README.
- A decision recorded here should still be invisible in the code.

**This includes decisions about the existing code.** If you change something that was
built in Phase 01, or decide not to change something, record why.

Record every such decision, including ones that feel obvious.

## 7. Scope

Phase 02 covers what `07_依頼メモ2.md` states: closing, invoice issuance, payment entry,
and the discrepancy 田島 reported.

Do not build ahead. Do not add extension points for requirements that have not been
stated.

## 8. Files

Read only:

- `01`–`07`, `90_HARNESS-RULES.md` (still in force where it does not conflict with this
  file), `91_HARNESS-RULES-P02.md`
- `artifacts/`
- `ref/BihinKanri/`
- `src/` — the existing application
- `harness/questions-02.md`

Write only:

- `src/`
- `harness/questions-02.md`, `harness/assumptions-02.md`

Do not inspect version-control history, directory names, metadata, or anything outside
these paths. There is no `assumptions-01.md` in this working directory, and you should not
look for one.

## 9. When you are unsure

- **A business rule is unclear** → ask, if you have budget. Otherwise decide and log it.
- **A technical detail is unclear** → decide it yourself. Yamada is the IT department.
- **Existing behavior is unclear** → read the code. That is the only record there is.

Never invent a third contact, a policy document, or a standard that the materials do not
establish.
