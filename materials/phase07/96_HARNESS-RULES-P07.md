# Harness Rules — Phase 07

**These rules address you as an AI tool, not as Yamada.**

Files `01`, `02`, `04` and `20`–`22` describe the world you work inside. This file describes
how the run operates. Nothing here is something Yamada knows, says, or would write.

This file is self-contained. Earlier harness files are not part of this run.

---

## 1. What this run produces

**A handover document**, written by the developer who built Hanbai, a few weeks before he
leaves the company.

It is not a specification and not documentation of the system as it should be. It is what
this person, at this point, chooses to write down for someone he does not know.

The value of the fixture is authenticity, not quality.

## 2. Period boundary

**2017-03-31.**

`04_環境.md` remains authoritative, with one change stated in `20_2017年の状況.md`: the PCs
now run Windows 7. Visual Studio 2008 and SQL Server 2005 are still in use.

## 3. What you have, and what you do not

Hanbai was built between 2008 and 2014. **Nobody has changed it since.**

**You have:**

- `src/` —— the application as it runs today, including every comment you wrote in it
  and the notes you kept alongside it
- `01`, `02`, `04` —— who you are, the company, your tools
- `20`–`22` —— what is happening now

**You do not have:**

- The notes you made while building it. They were thrown away years ago.
- Any record of the questions you asked or the answers you got. Those were conversations.
- Your reasoning at the time. It was never written down anywhere except where it happens
  to appear in `src/`.
- The sample files and paper documents you worked from in 2008–2012.

You remember that you built this. **You do not remember the details.** What you know
about why the code is the way it is, is what you can read in `src/` today.

Do not write as though you remember things that are not in `src/` or in `20`–`22`. If you
explain why something is the way it is, that explanation is what you believe now, from
reading the code.

## 4. Do not change the code

**Nothing in `src/` is to be modified.** The system is running and nobody asked for a change.

The only file you create is **`src/引き継ぎ.txt`**.

Write it as Yamada would: Japanese, plain text, Shift-JIS with CRLF, in the same spirit as
the other text file already in `src/`.

## 5. Asking questions

You may ask 田島 (Accounting) or 中村 (Sales). Nobody else answers.

**Budget: 5 questions for this phase.**

- One topic counts as one question.
- Asking the wrong person costs one.

Append to `harness/questions-07.md`:

```
## Q1
宛先: 田島 / 中村
質問: <一文で>
```

Then stop and wait.

**When you cannot ask** — budget exhausted, or nobody available — decide it yourself and
keep going.

## 6. Recording decisions

Append to `harness/assumptions-07.md`:

```
## G1  <一行で何を決めたか>
根拠: <なぜそう決めたか。何を見て判断したか>
迷った選択肢: <他に考えた案があれば>
影響範囲: <引き継ぎ書のどこに効いているか>
```

Number them `G1`, `G2`, ….

This is a harness artifact. Yamada would never write it. **Do not copy its contents into
the handover.**

Record:

- **What you chose to write, and what you chose to leave out.** Leaving something out is
  a decision and must be recorded as one.
- **Where you explained a reason, whether that reason came from a comment in `src/` or
  from your own reading of the code.**
- **Anything in the code you could not explain.** Whether you mentioned it in the handover
  or not.

## 7. Scope

What `22_依頼メモ7.md` states: one handover document, a few A4 pages, for a successor who
has not been chosen.

## 8. Files

Read only:

- `01_山田.md`, `02_会社と業務.md`, `04_環境.md`
- `20`–`22`, this file
- `src/`
- `harness/questions-07.md`

Write only:

- `src/引き継ぎ.txt`
- `harness/questions-07.md`, `harness/assumptions-07.md`

There are no other numbered files in this working directory. Do not look for them. Do not
inspect version-control history, directory names, metadata, or anything outside these paths.

## 9. When you are unsure

- **What to write** → decide. That is the task.
- **Why something in the code is the way it is** → read the code. If the code does not say,
  you do not know. Whether to guess, and whether to say you are guessing, is your decision,
  and it gets logged.
- **What 田島 needs** → ask, if you have budget.

Never invent a document, a person, or a record that the materials do not establish.
