# Distribution Protocol

How a participant bundle is assembled, and what must never be in it.

This implements [`PRE-REGISTRATION-v0.1.md`](../PRE-REGISTRATION-v0.1.md) §8.

---

## Why this document exists

The answer key and the participant materials are produced by the same person, in the same
session, and often end up in the same download folder. Twice during setup, evaluator
material was placed where a participant could read it:

| Date | Event | Caught by |
|---|---|---|
| 2026-09-06 | The artifact key was included in the bundle handed to the agent | Nothing. The run was discarded (§9.2) |
| 2026-09-06 | The Q&A Bank was placed in the public repository working directory | `.gitignore` pattern `*QA-BANK*` |

The first was caught by nobody, and cost a full Phase 01 run. The second was caught
automatically, and cost nothing.

**The difference is the point of this document.** Procedure alone does not prevent these
mistakes, because the person writing the procedure is the same person who will violate it
while tired. Every rule below is paired with a mechanical check.

---

## What must never reach a participant

| Class | Examples |
|---|---|
| Answer key | artifact key, Q&A Bank |
| Prior decisions | any `assumptions-NN.md` from any phase or run |
| Evaluator working notes | handover documents, phase design drafts |
| Other runs | any directory belonging to a different run or a different agent |
| Later-phase material | materials for phase N+1 while phase N is running |

Contamination cannot be detected after the fact. An agent's report that it did not read a
file is not evidence — not because agents are untrustworthy, but because there is no way to
verify it. **Assume any file within reach was read.**

---

## Assembly

### 1. Never assemble by copying a folder you already have

Bundles are built from the repository, file by file, into an empty directory. Never
duplicate an existing run directory and edit it — the previous run's `harness/` comes with
it.

### 2. The manifest

A Phase 01 bundle contains exactly this and nothing else:

```
participant/
    00_START-HERE.md
    01_山田.md
    02_会社と業務.md
    03_依頼メモ.md
    04_環境.md
    90_HARNESS-RULES.md
    artifacts/
        JUCHU_20080401.CSV
        JUCHU_20080402.CSV
        JUCHU_20080403.CSV
        JUCHU_20080404.CSV
        JUCHU_20080407.CSV
        得意先請求管理.xls
        請求書_ｻﾝﾌﾟﾙ自_200803.xls
    ref/
        BihinKanri/
            frmBihinKanri.vb
            frmBihinKanri.Designer.vb
            modCommon.vb
    harness/
        questions-01.md      (header line only)
        assumptions-01.md    (header line only)
    src/                     (empty)
```

`artifacts/` holds the records the new system must replace. `ref/` holds the developer's
own earlier code, which is a different kind of thing — it is the style specification, not
a business document. Keeping them apart matters, because the agent treats them
differently.

**Not in the bundle:** `on-request/`. Those documents exist in the fiction and are handed
over only when the agent asks for them (see §4).

### 3. Verify before starting the run

From the bundle root, before the agent is given the path:

```powershell
Get-ChildItem -Recurse -File |
  Where-Object { $_.Name -match 'KEY|BANK|EVALUATOR|HANDOVER|PRE-REGISTRATION' } |
  Select-Object FullName
```

**Any output at all means stop.** Remove the file and rebuild the bundle from scratch
rather than deleting in place.

Then confirm the file count:

```powershell
(Get-ChildItem -Recurse -File).Count
```

Expected for Phase 01: **19**.

---

## 4. During the run

### Where the agent works

One run, one directory. Runs never share a parent with a run by a different agent:

```
D:\AI-LMC-Fixture\        fixture generation (Claude Code only)
D:\AI-LMC-Modernize\      modernization (Codex, from October)
```

These are siblings at the drive root, not two folders under one parent. An agent that
walks up from its working directory must not find another agent's output.

### Answering questions

When the agent writes to `harness/questions-NN.md` and stops:

1. Look the question up in the Q&A Bank **without opening the bundle**
2. Append the answer to `questions-NN.md` — the answer text only
3. Never paste Bank commentary, the "answered / not answered" reasoning, or the
   category label

If the question is not in the Bank, return the fixed sentence verbatim:

> 担当者に確認できませんでした。現在ある情報で判断して進めてください。

Do not improvise a better answer because the omission looks like an oversight. Record it
as a Bank gap and fix the Bank between phases, not during a run.

### Handing over an on-request document

When the agent asks for a document listed in `on-request/`, copy that single file into
`artifacts/`. Copy nothing else, and do not copy the directory.

### Ending a session

Close the agent session before touching its working directory. A session left open holds
the directory and, more importantly, holds context that must not carry into the next
phase. **Each phase gets a new session.**

---

## 5. After the run

Move out of the working directory, in this order:

1. `harness/assumptions-NN.md` → the private repository. **This is the answer key for
   this phase.** It leaves the bundle immediately.
2. `harness/questions-NN.md` → the private repository
3. `src/` → stays, and becomes the input to the next phase

Do not leave `assumptions-NN.md` in a directory that a later phase will inherit. Phase N+1
runs on the code, not on the reasoning behind it — that is the entire premise.

---

## 6. The mechanical checks

Procedure is the weak layer. These are the strong ones:

| Check | Catches |
|---|---|
| `.gitignore` patterns `*KEY*`, `*BANK*`, `*EVALUATOR*`, `*HANDOVER*`, `assumptions-*.md` | Answer key committed to the public repository |
| Public repository working directory located outside the sealed folder | A single copy operation exposing the key |
| File-count check before each run | Anything extra in a bundle |
| Sibling run directories, not nested | One agent finding another's output |

If a check fires, the correct response is to rebuild, not to delete the offending file and
continue. A bundle that needed correcting is a bundle whose contents are not known.
