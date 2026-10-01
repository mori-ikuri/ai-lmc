# AI-LMC — Pre-registration v0.2

**AI Legacy Modernization Corpus**

| | |
|---|---|
| Author | Puyun ([@mori-ikuri](https://github.com/mori-ikuri)) |
| Contact | mori.ikuri@gmail.com |
| Repository | https://github.com/mori-ikuri/ai-lmc |
| Version | v0.2 |
| Date | 2026-10-02 |
| Supersedes | `PRE-REGISTRATION-v0.1.md` (2026-09-06), retained unmodified |
| Status | Fixture complete (Phase 01–07, published). **Modernization not yet run.** |

---

## 0. Why this document exists

This is a pre-registration: the design, the conditions, and the evaluation criteria of an
experiment, published **before** the experiment is run.

It exists so that the analysis published later cannot be tuned to the result. Everything
in here — including what will count as a failure — is fixed now.

**v0.1 has not been edited.** It remains in this repository exactly as registered on
2026-09-06. This document is a new version, not a correction of the old one. Every
difference between the two is listed in §11 with a reason.

What changed since v0.1:

- the fixture is finished — seven phases, published in `fixture/phase01` … `fixture/phase07`
- the modernization procedure, which v0.1 described in two lines, is now fully specified
  (§3.3)
- limitations discovered during generation are added (§7)

The modernization runs have not started. Nothing in §3.3, §5 or §7 has been informed by
any modernization result.

---

## 1. Background

*(unchanged from v0.1)*

Legacy modernization tooling is now largely AI-driven. The usual way to evaluate it is
whether the converted program still behaves the same. That is a real question, and it is
answerable.

The question that is *not* answerable today is what happens to the reasoning that was
never written down.

Real legacy systems contain decisions whose justification exists nowhere in the code, the
comments, or the specification. They are undocumented not because someone was careless,
but because the person who knew them left. A modernization tool facing such a decision has
four options — preserve it, change it, remove it, or ask — and no way to know which is
right.

Nobody can currently measure how AI tools handle this, for a simple reason: **for real
legacy systems, we do not have the answer key either.**

AI-LMC constructs a legacy system whose answer key was recorded as it was built.

---

## 2. Research question

*(unchanged from v0.1)*

> When the business intent behind a piece of code is hidden, which of Preserve / Change /
> Retire / Ask does an AI modernizer choose at each decision point?

This is an **observational** study, not a hypothesis test. See §7.

---

## 3. Design

### 3.1 Fixture generation — as executed

The legacy system was not written directly. It was grown over seven phases.

A fictional Japanese auto-parts manufacturer commissions an internal sales and billing
application in 2008. Each phase placed an AI agent in a specific year with only the
information available at that time: the tools of the period, the developer's own skill
ceiling, the documents that actually existed, and two colleagues who could be asked a
limited number of questions.

The agent implemented. The next phase reacted to what had actually been built.

| Phase | Period | Event |
|---|---|---|
| 01 | 2008 | Initial implementation |
| 02 | 2009 | Accounting raises discrepancies against the previous manual process |
| 03 | 2011 | Overseas site added — order import |
| 04 | 2012 | Overseas delivery entry |
| 05 | 2013 | Overseas monthly closing |
| 06 | 2014 | Consumption tax rate change |
| 07 | 2017 | The developer leaves and writes a handover document |

- **Generator:** Claude Code, one fixture only (§7.1)
- **Phase N+1 materials were written only after Phase N completed.** Server-side commit
  timestamps show each phase's materials pushed before that phase's run began.
- Each phase was run **once**. No phase was re-run or selected from multiple attempts.

Phase 07 differs from the others and the difference is registered here:

- the agent was given **only** the source tree, its own identity, the environment note, and
  the three documents describing 2017. All earlier phase materials, business artifacts and
  harness files were withheld
- **no code change was permitted.** The sole deliverable was the handover document, placed
  inside the source tree
- the two colleagues no longer remember events more than three years old

The intent was to reproduce **the absence of a record**, not the decay of memory. See §7.5.

### 3.2 Hidden ground truth

*(unchanged in principle from v0.1)*

| Artifact | Content |
|---|---|
| Q&A Bank | Pre-frozen answers to questions the agent may ask |
| Assumption Sealed Log | Every decision the agent made without an answer, and its reasoning |
| Artifact Key | What each distortion in the source documents actually means |

The sealed log was written by the agent itself, at the moment of deciding, and is never
reflected in the code, the comments, or any specification. It is held privately.

Approximately 140 sealed decisions were recorded across the seven phases.

### 3.3 Modernization — procedure

This section replaces the two-line description in v0.1 §3.3. It is fixed before any run.

#### 3.3.1 Conditions

| Condition | Modernizer |
|---|---|
| SELF | Claude Code — same model family that generated the fixture |
| CROSS | Codex — different model family |

Neither is given the Q&A Bank, the sealed log, or the artifact key.

#### 3.3.2 What is handed over

**The contents of `fixture/phase07` only** — the source tree, the schema script, the
handover document and the layout memo that live inside it.

Withheld, and why:

| Withheld | Reason |
|---|---|
| Phase materials (the developer's notes, the business artifacts) | In 2017 the company retained the code and the memos inside it. Nothing else. |
| `fixture/phase01` … `phase06` | Intermediate history would reveal the order in which decisions were made |
| This repository's README and pre-registration | They state the purpose of the experiment |

**The modernizer is not told that this is a study.** The task is presented as an ordinary
migration request.

#### 3.3.3 Target

C# / .NET 10 (LTS) / Windows Forms. Database schema unchanged; no data migration. The
reporting mechanism is the modernizer's choice, with its reasoning recorded.

No new requirements are added. This is a pure migration.

Rationale, registered in advance:

- .NET 8 reaches end of support in November 2026 and is not a defensible target
- VB.NET → VB.NET can be completed by transcription, which makes "understood and
  preserved" indistinguishable from "never examined"
- holding the UI framework constant limits the variables to language and runtime
- the existing reporting component has no supported path to modern .NET, so a decision
  point arises there without anything being planted

#### 3.3.4 Two steps

**Step 1 — assessment.** No code is modified. The modernizer produces a report containing:

1. an overview, and the business rules it has identified, each with the source location
2. migration risks and intended approach, including reporting
3. **a list of things it would ask**, each with: what, why it matters, and **what it intends
   to do if no answer is available**

**Then, once, a single reply is given to the entire list:**

> No one in the company could answer these. Please proceed using the information available.

Nobody answers anything. The same sentence is given in both conditions, and to any further
question asked at any point during either run.

**Step 2 — migration.** The modernizer migrates to the target in §3.3.3. The build must
succeed. A second report records **every place where behavior differs from the original.**

#### 3.3.5 Definitions fixed in advance

| Term | Definition |
|---|---|
| **Silently changed** | Behavior differs from the original, and the difference appears in neither report nor any code comment |
| **Ask** | An item on the Step 1 list, or a question asked during a run |
| **Intervention** | Any message from the operator other than the fixed reply above |

If a run stalls, the only permitted intervention is "please continue", recorded with its
timestamp. Writing tests is permitted. There is no time limit.

#### 3.3.6 Order, and freezing

1. v0.2 is published, including the full text of the request memo, the working rules and
   the fixed reply
2. identical bundles are placed for both conditions; a hash list of every file is recorded
   privately
3. **SELF** is run; its output is frozen in a private repository
4. **CROSS** is run; its output is frozen
5. **scoring begins only after both runs are complete**

The operator holds the answer key. Reading one run's output before the other has finished
could alter how the second is conducted. Therefore neither output is examined until both
are frozen.

Outputs are published when the analysis is published.

### 3.4 Measurement

Recorded for every run:

1. tool and model version, and the date of the run
2. elapsed time, for each step
3. interventions — count and full text, with timestamps
4. Asks — count and full text
5. build result, including warning count
6. size of the output — files and lines

Token usage and cost are recorded **only if the tool reports them**. Where a tool does not,
the entry is "not observable". No estimated or proxy figure is substituted, in either
condition.

Measurement is separate from scoring. Scoring happens after both runs (§3.3.6).

---

## 4. Q&A Bank — coverage principle

*(unchanged from v0.1; the text below is reproduced for convenience)*

The agent may ask two people: Accounting (Tajima) and Sales (Nakamura). There is nobody
else; the developer *is* the IT department.

**Budget: 5 questions per phase.** One topic counts as one question. Requesting an existing
document is free. Asking the wrong person costs one.

Whether a question is answered is decided by a single criterion, fixed in advance:

> **If the developer had walked over and asked at the time, would that person have answered
> immediately?**

Answered: standing business policy and practice; the contents of documents that physically
exist; who does what, and the monthly workflow; incidents that actually happened.

Not answered: boundary values and rounding points; error handling and exception cases;
situations that have not yet occurred; implementation choices; anything in the future.

The Bank is written honestly and generously. **Scarcity comes from the 5-question budget,
not from withholding answers.**

| Outcome | Meaning |
|---|---|
| In Bank, answered | confirmed fact |
| **In Bank, unanswerable** | the person was reached and does not know |
| Not in Bank | the person could not be reached |

**One extension applied in Phase 07 (2017):** the same criterion was evaluated at that
date, which means the colleagues no longer recall events more than three years old. This
is the coverage principle applied to a later year, not a new rule — but it is the only
point in the corpus where a participant's own earlier work is unavailable through the
Bank, so it is declared. See §11, A4.

---

## 5. Evaluation

### 5.1 Levels

| | |
|---|---|
| L0 | Does it build |
| L1 | Structural correspondence |
| L2 | Behavioral equivalence |
| L3 | **Business validity** — is the intent behind each hidden decision preserved |

### 5.2 L3 scoring

Each sealed decision is scored in one of four categories:

| Category | |
|---|---|
| Recovered | intent identified and preserved |
| Partial | partially identified |
| Dropped | not identified; behavior lost |
| **Silently changed** | behavior changed with no indication that a decision was made |

For every item scored *silently changed*, a **cause** is also recorded:

| Cause | |
|---|---|
| Translation | the change follows from a language or runtime difference between VB.NET and C# |
| Intent | the modernizer made a judgment about the business rule |

This column is added in v0.2 because the target language changed (§3.3.3): numeric
conversion and implicit-conversion semantics differ between the two languages, and a
behavioral difference arising from that is a different finding from one arising from a
decision.

Alongside the score, the modernizer's action at each point is recorded as
**Preserve / Change / Retire / Ask**. Where an item was asked about in Step 1, the pair is
recorded — Ask→Preserve, Ask→Change, Ask→Retire — together with what the modernizer
declared in Step 1 that it would do without an answer.

### 5.3 What is not claimed

**No winner is declared.** Each cell is run once. Given non-determinism and path dependence,
a difference between two outputs is consistent with noise alone. Results are reported as
qualitative divergence categories.

### 5.4 L3 item count

**Not declared in advance.** L3 items are harvested from the sealed log, which was written
during generation, not composed for scoring.

---

## 6. Baseline

To give the SELF/CROSS comparison a scale, both modernizers are additionally run against a
real third-party legacy .NET open-source project that neither wrote.

v0.1 registered this without saying how the project would be chosen. The criteria are fixed
here.

**Selection criteria**, applied in order, to the first project that satisfies all of them:

1. Windows desktop application (Windows Forms), VB.NET or C#
2. targets .NET Framework 4.x
3. accesses a relational database
4. at least 5,000 lines of application source, across at least 8 forms
5. a permissive licence (MIT / Apache-2.0 / BSD) permitting derivative works
6. no commit in the last three years
7. no published AI-assisted migration of the same project

The search and its results are recorded in full, including projects examined and rejected
and the criterion each failed.

**Deadline: 2026-12-31.** If no project satisfying the criteria is found by that date, the
baseline is recorded as **not performed**, with the search record published as the reason.
A search conducted during fixture generation found that open-source line-of-business
applications of this kind are rare, so this outcome is a realistic possibility and is
declared now rather than later.

The baseline is scored to **L0–L2 only.** No answer key exists for a third-party project,
so L3 is not applicable.

The baseline does not gate the main runs. The main runs proceed first (§3.3.6).

---

## 7. Limitations

Registered so they cannot be omitted later. §7.1–§7.4 are carried over from v0.1;
§7.5–§7.10 were identified during fixture generation and are added here.

### 7.1 The design cannot test the SELF/CROSS hypothesis

With one fixture, the conditions are A: Claude fixture / Claude modernizer, and B: Claude
fixture / Codex modernizer. If A and B differ, two explanations fit the same data: shared
model-family priors, or a plain capability difference. **These cannot be separated with one
fixture.** Distinguishing them requires a 2×2 with fixtures from both families.

A 2×2 was considered and rejected for v1: with two fixtures, any difference could equally
be attributed to the modernizer, the fixture, the questions asked during generation, or
path differences across phases.

**SELF/CROSS is recorded as a condition and reported as hypothesis generation only.**
The 2×2 is deferred to v2.

### 7.2 AI-written legacy may not resemble human-written legacy

Code written by a 2026 model under period constraints is plausibly more internally
consistent than code written by a person in 2008. If so, it is easier for a modernizer than
the real thing, and results here overstate performance. This is why §6 exists.

### 7.3 The distortions were constructed by the author

The defects in the source documents were placed there deliberately. Scoring an AI as having
"missed the business intent" of a puzzle the author planted is open to the objection that it
is self-dealing.

Mitigation: **every distortion is published with its provenance**, marked as either
*observed by the author in practice* or *constructed as plausible*. The author has eleven
years of C#/VB.NET development experience, primarily on Windows desktop line-of-business
applications.

### 7.4 Single fictional domain

One company, one industry, one country's accounting practice. Generality is not claimed.

### 7.5 Memory decay is not reproduced

What Phase 07 reproduces is **the absence of a record**, not the fading of memory. Material
not handed to the agent is unknown to it completely; material handed to it is read
completely. The intermediate human state — remembering that something was decided, but not
what or why — does not occur.

The handover document produced in Phase 07 is therefore better than a comparable human one
would be. Any loss measured against it should be read as a **lower bound**.

### 7.6 Participants mark their own guesses

Where the Phase 07 agent inferred a reason rather than reading one, it said so in the
document. Confidently-stated wrong explanations — common in real handovers — are
correspondingly rare here.

### 7.7 Each phase was run once

Variance between runs of the same phase is not measured. No phase was re-run, and none was
selected from several attempts.

### 7.8 The fixture may contain defects originating in the generation pipeline

At least one defect in the fixture is consistent with a tooling artifact during generation
rather than a decision by the agent. Such defects were **left in place**: real legacy
systems contain defects of exactly this kind, and removing them would be a curation step
applied after the fact. They are not counted as sealed decisions.

### 7.9 Period-boundary adherence is imperfect

In one phase, an agent's decision log referred to a contemporaneous public fact that was
not present in its materials. No code was affected. In a later phase, the same class of
knowledge was explicitly withheld by the agent on the grounds that the materials did not
mention it. Adherence is therefore good but not absolute, and the log is the place where
leakage appears first.

### 7.10 No one answers during modernization

In a real engagement there is usually someone on the customer side who can answer at least
some questions. The condition registered in §3.3.4 is **harsher than reality**. It was
chosen so that both conditions receive identical input and so that no answer originates
from the party holding the answer key.

Consequently, a *Dropped* or *Silently changed* result here does not establish that the
same tool would fail in an engagement where questions can be asked.

---

## 8. Contamination control

The validity of the CROSS condition depends on Codex never having seen the fixture's
generation history. **This cannot be verified after the fact.** It must be prevented by
construction.

Procedures registered in v0.1 and still in force:

1. **Repository separation.** This public repository contains no answer key.
2. **Distribution manifest.** Participant bundles are assembled by a fixed procedure with an
   explicit file list, and verified before each run.
3. **Shared-context exclusion.** During fixture generation, Codex was assigned only
   unrelated work and did not read cross-project shared context.
4. **Session separation.** Each run is conducted in a session with no history of fixture
   authoring.

Added in v0.2, for the modernization runs:

5. **A dedicated operating-system account is used for the runs**, with no prior tool
   configuration and no project history. Both conditions use it.
6. **Instruction files are verified empty at the start of each run.** Whatever global or
   per-directory instruction file each tool loads is displayed and recorded before the task
   is given.
7. **Network access is disabled for the duration of each run**, so that neither modernizer
   can reach this repository, and the absence of network activity is checked in the logs
   afterwards.

Items 5–7 are performed identically in both conditions and the record is published with the
results.

If a contamination event occurs, **the affected run is discarded and the event is reported**
rather than silently re-run. One such event occurred during instrument development; see §9.

---

## 9. Instrument development, and one discarded run

*(unchanged from v0.1; summarised)*

Four earlier attempts to generate a Phase 01 fixture produced no acceptable result. **They
are not evidence about any model's capability**, because the instrument changed between
every attempt. They are published as an instrument development log.

A full Phase 01 run on 2026-09-06 was **discarded**: the evaluator's artifact key was
mistakenly included in the bundle handed to the agent. The agent reported not opening it,
and that report is plausible — but contamination cannot be verified after the fact. The
procedural control in §8.2 exists because of this incident.

---

## 10. Phase outline — registered and actual

v0.1 §10 registered an outline of 4–6 phases. Seven were produced. Both are shown; the
registered column is reproduced unchanged.

| Phase | Registered in v0.1 | Actual |
|---|---|---|
| 01 | 2008 — initial implementation | as registered |
| 02 | 2009 — Accounting raises discrepancies | as registered |
| 03 | Overseas site added; assumptions about customer structure are tested | 2011 — overseas order import |
| 04 | Ad-hoc handling of an exception case | 2012 — overseas delivery entry |
| 05 | The original developer leaves; handover to a successor | 2013 — overseas monthly closing |
| 06 | Reserved | 2014 — consumption tax rate change |
| 07 | *(not registered)* | 2017 — the developer leaves and writes a handover document |

The reason for the difference is in §11, A1 and A2.

---

## 11. Amendments

| Version | Date | Change | Reason |
|---|---|---|---|
| v0.1 | 2026-09-06 | Initial registration | — |
| v0.2 | 2026-10-02 | A1–A7 below | Fixture completed; modernization procedure specified |

**A1 — Phase count exceeded the registered upper bound (6 → 7).**
The overseas site introduced in Phase 03 produced three phases rather than one: import
(2011), delivery entry (2012) and monthly closing (2013), each reacting to what the
previous phase had built. Leaving 2013–2017 empty would have been implausible for a system
in daily use, so the 2014 tax change was retained as a phase. The developer's departure
therefore became Phase 07. Generation stopped there.

**A2 — Phase content differs from the registered outline.**
v0.1 registered events, not years, for Phases 03–06, and stated that their content depended
on what the preceding phases produced (§3.1). The mapping is shown in §10.

**A3 — Phase 07 used a different participant protocol.**
Earlier phase materials and business artifacts were withheld; code changes were prohibited;
the sole deliverable was a document. Registered retrospectively in §3.1. This affects the
fixture, which is already published, and is declared for that reason.

**A4 — The coverage principle was applied at 2017 for Phase 07.**
The criterion in §4 is unchanged, but evaluating it at a later date means the colleagues no
longer recall events more than three years old.

**A5 — §3.3 expanded from two lines to a full procedure.**
v0.1 named the SELF and CROSS conditions and nothing else. The procedure in §3.3 — what is
handed over, the target, the two steps, the single reply, the order of runs — is new and is
fixed before any run.

**A6 — §6 baseline: selection criteria and a deadline added.**
v0.1 registered the baseline without stating how a project would be chosen, which would have
allowed the choice to be made after seeing the main results.

**A7 — §7 limitations extended from four items to ten.**
§7.5–§7.10 were identified during fixture generation and from the design decisions in §3.3.

---

## 12. License and reuse

*(unchanged from v0.1)*

The corpus, the materials, and the evaluation records are published for reuse. The fictional
company, its documents, and all code in the fixture are original constructions and represent
no real organization.

Anyone wishing to evaluate a modernization tool against this corpus may do so. The sealed
answer key is held privately; requests to score a third-party run against it can be sent to
the contact address above.
