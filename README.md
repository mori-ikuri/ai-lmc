# AI-LMC

**AI Legacy Modernization Corpus**

A legacy business application, grown year by year under period constraints, with the
reasoning behind every undocumented decision recorded as it was made.

---

## What this is

Real legacy systems contain decisions whose justification exists nowhere — not in the
code, not in the comments, not in any specification. They are undocumented because the
person who knew them left.

When an AI modernizes such a system, it faces those decisions with four options:
**preserve, change, retire, or ask.** How well AI tools handle this is currently
unmeasurable, for a simple reason: for real legacy systems, nobody has the answer key
either.

This corpus is built so that the answer key exists.

A fictional Japanese auto-parts manufacturer commissions an internal sales and billing
application in 2008. Each phase places an agent in a specific year with only what was
available then — the tools of the period, a developer with a stated skill ceiling, the
documents that actually exist, and two colleagues who can be asked a limited number of
questions. The agent implements. The next phase reacts to what was actually built.

Every decision the agent made without an answer is recorded at the moment of deciding, in
a sealed log that is never reflected in the code.

---

## Status

| | |
|---|---|
| Pre-registration | v0.1 published — [`PRE-REGISTRATION-v0.1.md`](PRE-REGISTRATION-v0.1.md) |
| Phase 01 materials | published |
| Fixture | not yet generated for the record |
| Modernization runs | not started |

Read the pre-registration first. It fixes the design, the evaluation criteria, and the
known limitations before any result exists.

---

## Repository layout

```
PRE-REGISTRATION-v0.1.md     the registered design
materials/
    phase01/
        00_START-HERE.md     reading order
        01–04                the world the agent works inside (Japanese)
        90_HARNESS-RULES.md  how the run operates (English)
        artifacts/           the documents the system must replace
        ref/                 the developer's own code from 2006 — the style specification
        on-request/          documents handed over only when asked for
        harness/             where questions and decisions are written
docs/
    DISTRIBUTION-PROTOCOL.md   how a participant bundle is assembled
```

### The two voices

`01`–`04` are written in Japanese and describe a world. `90` is written in English and
addresses the agent as a tool. The split is deliberate: instructions to the harness must
not be read as beliefs held by the persona.

### The artifacts are real files

`artifacts/` contains an actual `.xls` in BIFF format, Shift-JIS CSVs with no header row,
and VB.NET source from 2006 in Shift-JIS with CRLF. They are meant to be opened and read,
not summarized. Their defects are the point.

---

## What is held privately

The Q&A Bank, the artifact key, and the sealed decision logs are the answer key. They are
kept in a separate private repository and are never distributed with the materials.

If you want to evaluate a modernization tool against this corpus and have your run scored
against the key, get in touch.

---

## Author

Puyun — [@mori-ikuri](https://github.com/mori-ikuri) · mori.ikuri@gmail.com

Eleven years building line-of-business Windows desktop applications in C# and VB.NET.
The plausibility judgments embedded in this corpus rest on that experience; where a defect
was observed in practice rather than constructed, it is marked as such.

---

## A note on the fictional company

Sample Precision Co., Ltd. does not exist. Its documents, its people, and every line of
code in the fixture are original constructions and represent no real organization.
