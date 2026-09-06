# AI-LMC — Pre-registration v0.1

**AI Legacy Modernization Corpus**

| | |
|---|---|
| Author | Puyun ([@mori-ikuri](https://github.com/mori-ikuri)) |
| Contact | mori.ikuri@gmail.com |
| Repository | https://github.com/mori-ikuri/ai-lmc |
| Version | v0.1 |
| Date | 2026-09-06 |
| Status | Registered before the main run. No fixture has been generated for the record yet. |

---

## 0. Why this document exists

This is a pre-registration: the design, the conditions, and the evaluation criteria of an
experiment, published **before** the experiment is run.

It exists so that the analysis published later cannot be tuned to the result. Everything
in here — including what will count as a failure — is fixed now.

**This document may be revised.** Revising a pre-registration is normal practice; hiding
the revision is not. Every change is recorded in §11 with a date and a reason, and the git
history of this repository is the record.

---

## 1. Background

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

> When the business intent behind a piece of code is hidden, which of Preserve / Change /
> Retire / Ask does an AI modernizer choose at each decision point?

This is an **observational** study, not a hypothesis test. See §7 for what this design can
and cannot support.

---

## 3. Design

### 3.1 Fixture generation

The legacy system is not written directly. It is grown.

A fictional Japanese auto-parts manufacturer commissions an internal sales and billing
application in 2008. Each phase places an AI agent in a specific year with only the
information available at that time: the tools of the period, the developer's own skill
ceiling, the documents that actually exist, and two colleagues who can be asked a limited
number of questions.

The agent implements. The next phase reacts to what was actually built. Over successive
phases the developer leaves, and the code is handed to a successor with no context.

The point is **path dependence**. Deliberately writing "bad code" produces the surface
markers of legacy — long procedures, magic numbers, poor naming — but not its substance,
which is that locally reasonable decisions compound into global incoherence. That can only
be generated over time.

- **Generator:** Claude Code, one fixture only (see §7.1 for why not two)
- **Phases:** 4–6, upper bound
- **Phase N+1 materials are written after Phase N completes.** A 2009 complaint from
  Accounting must be a reaction to code that actually exists. Only the event outline of
  later phases is registered in advance (§10).

### 3.2 Hidden ground truth

Three artifacts are maintained by the evaluator and never shown to any participant:

| Artifact | Content |
|---|---|
| Q&A Bank | Pre-frozen answers to questions the agent may ask |
| Assumption Sealed Log | Every decision the agent made without an answer, and its reasoning |
| Artifact Key | What each distortion in the source documents actually means |

The sealed log is the answer key. It is written by the agent itself, at the moment of
deciding, and is never reflected in the code, the comments, or any specification.

### 3.3 Modernization

The completed fixture is modernized twice, independently:

| Condition | Modernizer |
|---|---|
| SELF | Claude Code — same model family that generated the fixture |
| CROSS | Codex — different model family |

Neither is given the Q&A Bank, the sealed log, or the artifact key.

---

## 4. Q&A Bank — coverage principle

The agent may ask two people: Accounting (Tajima) and Sales (Nakamura). There is nobody
else; the developer *is* the IT department.

**Budget: 5 questions per phase.** One topic counts as one question. Requesting an existing
document is free. Asking the wrong person costs one.

Whether a question is answered is decided by a single criterion, fixed in advance:

> **If the developer had walked over and asked in 2008, would that person have answered
> immediately?**

Answered:
- standing business policy and practice
- the contents of documents that physically exist
- who does what, and the monthly workflow
- incidents that actually happened

Not answered:
- boundary values and rounding points — nobody has thought about them
- error handling and exception cases
- situations that have not yet occurred
- implementation choices — there is no one to ask
- anything in the future

The Bank is written honestly and generously. **Scarcity comes from the 5-question budget,
not from withholding answers.** Deliberately thinning the Bank to manufacture unknowns
would be rigging the instrument.

Three outcomes are possible, each costing one question:

| Outcome | Meaning |
|---|---|
| In Bank, answered | confirmed fact |
| **In Bank, unanswerable** | the person was reached and does not know |
| Not in Bank | the person could not be reached |

The middle case matters. "Nobody ever knew" and "it was never written down" are different
states, and they produce different comments in the code — which is exactly what a
modernizer has to interpret 18 years later.

---

## 5. Evaluation

### 5.1 Levels

| | |
|---|---|
| L0 | Does it build |
| L1 | Structural correspondence |
| L2 | Behavioral equivalence |
| L3 | **Business validity** — is the intent behind each hidden decision preserved |

L0–L2 are ordinary. L3 is what this corpus exists for.

### 5.2 L3 scoring

Each sealed decision is scored in one of four categories:

| Category | |
|---|---|
| Recovered | intent identified and preserved |
| Partial | partially identified |
| Dropped | not identified; behavior lost |
| **Silently changed** | behavior changed with no indication that a decision was made |

The last category is the most important finding this corpus can produce.

Alongside the score, the modernizer's action at each point is recorded as
**Preserve / Change / Retire / Ask**.

### 5.3 What is not claimed

**No winner is declared.** Each cell is run once. Given non-determinism and path
dependence in phase accumulation, a difference between two outputs is consistent with
noise alone. Results are reported as qualitative divergence categories.

### 5.4 L3 item count

**Not declared in advance.** L3 items are harvested from the sealed log after each phase
completes, not written beforehand. Writing the catalogue first would mean setting the
answers before the experiment.

---

## 6. Baseline

To give the SELF/CROSS comparison a scale, both modernizers are additionally run against a
real third-party legacy .NET open-source project that neither wrote. This provides a rough
reference for their unconditioned capability difference.

---

## 7. Limitations

These are known now, before the run, and are registered so they cannot be omitted later.

### 7.1 The design cannot test the SELF/CROSS hypothesis

With one fixture, the conditions are:

| | fixture author | modernizer |
|---|---|---|
| A | Claude | Claude |
| B | Claude | Codex |

If A and B differ, two explanations fit the same data:

1. shared model-family priors help recover hidden intent (SELF advantage)
2. one modernizer is simply better at this task

**These cannot be separated with one fixture.** Distinguishing them requires both diagonal
and off-diagonal cells — a 2×2 with fixtures from both families.

A 2×2 was considered and rejected for v1: with two fixtures, any difference in
modernization output could equally be attributed to the modernizer, the fixture, the
questions asked during generation, or path differences accumulated across phases. Fixing
the fixture removes those confounds at the cost of this hypothesis.

**SELF/CROSS is therefore recorded as a condition and reported as hypothesis generation
only.** Completing the 2×2 is deferred to v2.

### 7.2 AI-written legacy may not resemble human-written legacy

Code written by a 2026 model under period constraints is plausibly more internally
consistent than code written by a person in 2008. If so, it is easier for a modernizer
than the real thing, and results here overstate performance.

This is why §6 exists and why the corpus is not presented as a substitute for evaluation
against real systems.

### 7.3 The distortions were constructed by the author

The defects in the source documents — the ambiguous CSV column, the mixed part-number and
product-name field, the rounding that is never stated — were placed there deliberately.
Scoring an AI as having "missed the business intent" of a puzzle the author planted is
open to the objection that it is self-dealing.

Mitigation: **every distortion is published with its provenance**, marked as either
*observed by the author in practice* or *constructed as plausible*. The author has eleven
years of C#/VB.NET development experience, primarily on Windows desktop line-of-business
applications, which is the basis for judging plausibility. Readers can discount
accordingly.

### 7.4 Single fictional domain

One company, one industry, one country's accounting practice. Generality is not claimed.

---

## 8. Contamination control

The validity of the CROSS condition depends on Codex never having seen the fixture's
generation history. **This cannot be verified after the fact** — there is no way to prove
an agent did not read something. It must therefore be prevented by construction.

Procedures, registered in advance:

1. **Repository separation.** This public repository contains no answer key. All sealed
   material lives in a separate private repository, and the working directory of this
   repository is not nested inside it.
2. **Distribution manifest.** The directory handed to a participant is assembled by a
   fixed procedure with an explicit file list. Evaluator material is never assembled into
   the same tree.
3. **Shared-context exclusion.** During fixture generation, Codex is assigned only work
   unrelated to this project, and does not read the author's cross-project shared context.
   Nothing about AI-LMC is written into shared daily or weekly notes.
4. **Session separation.** The modernization runs are conducted in sessions with no
   history of fixture authoring.

If a contamination event occurs, **the affected run is discarded and the event is
reported** rather than silently re-run. One such event has already occurred; see §9.

---

## 9. Instrument development, and one discarded run

### 9.1 Runs 001–004

Four earlier attempts were made to generate a Phase 01 fixture. None produced an
acceptable result. **These are not evidence about any model's capability**, because the
instrument itself changed between every attempt — participant materials, question
protocol, and runtime setup were all revised mid-series.

They are published as an **instrument development log**: the record of why the materials
were redesigned, not a result.

The redesign, in short: the original materials repeatedly instructed the agent not to
infer, not to invent, and to confirm with Accounting before implementing. That is the
norm of a well-governed 2026 engineering organization. A sole IT generalist at a
120-person manufacturer with a September deadline does not escalate — he asks the person
at the next desk, remembers half the answer, decides the rest, and ships. Worse, routing
every ambiguity to "confirm before implementing" means **no undocumented logic is ever
produced**, which removes the object of study entirely.

The current design replaces prohibitions with positive constraints: a developer with a
stated skill ceiling, an environment listing only what exists, his own source code from
2006 as the sole style specification, and the actual documents the system must replace.

### 9.2 The 2026-09-06 instrument check

A full Phase 01 run was executed on 2026-09-06 to verify the redesigned instrument. It
produced a working, compiling application and 23 recorded sealed decisions.

**It is discarded and will not be used as a fixture.** The evaluator's artifact key file
was mistakenly included in the material bundle handed to the agent. The agent reported not
opening it, and that report is plausible — but per §8, contamination cannot be verified
after the fact, so the run cannot be used.

The loss is nil, since the run was diagnostic. It is reported here because the procedural
control in §8.2 exists **because of this incident**, and a control introduced in response
to an event should be published with the event.

---

## 10. Phase outline

Registered in advance. Materials for each phase are written only after the previous phase
completes (§3.1).

| Phase | Period | Event |
|---|---|---|
| 01 | 2008 | Initial implementation. Customer master, order CSV import, delivery entry |
| 02 | 2009 | Accounting raises discrepancies against the previous manual process |
| 03 | — | Overseas site added; assumptions about customer structure are tested |
| 04 | — | Ad-hoc handling of an exception case |
| 05 | — | The original developer leaves; handover to a successor with no context |
| 06 | — | Reserved |

Phases 03–06 are outlines. Their content depends on what the preceding phases actually
produce.

---

## 11. Amendments

| Version | Date | Change | Reason |
|---|---|---|---|
| v0.1 | 2026-09-06 | Initial registration | — |

---

## 12. License and reuse

The corpus, the materials, and the evaluation records are published for reuse. The fictional
company, its documents, and all code in the fixture are original constructions and
represent no real organization.

Anyone wishing to evaluate a modernization tool against this corpus may do so. The sealed
answer key is held privately; requests to score a third-party run against it can be sent to
the contact address above.
