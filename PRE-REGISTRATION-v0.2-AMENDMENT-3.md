# AI-LMC — Pre-registration v0.2, Amendment 3

|            |                                                                                                   |
| ---------- | ------------------------------------------------------------------------------------------------- |
| Date       | 2026-10-05                                                                                        |
| Applies to | `PRE-REGISTRATION-v0.2.md` and Amendments 1–2 (all retained unmodified)                           |
| Status     | Registered after L1–L2 judging of the v1 runs, **before any §6 baseline project is selected or run.** |

## Disclosure

§5.1 registers L1 (structural correspondence) and L2 (behavioral equivalence) by name only.
No method was registered for either.

The methods below were written on 2026-10-05, **after** both v1 outputs had been read for L3
scoring. They were committed to the private evaluation repository (commit `181fe6a`)
**before any L1 or L2 judgment was made**, and judging followed.

Consequently:

1. For the v1 runs, L1 and L2 are **post hoc definitions.** Wherever v1 L1–L2 results
   appear, they are labelled as defined after the outputs were examined, together with
   this amendment. CROSS run001 remains exploratory under A12.
2. For the §6 baseline, which has not been run, this amendment **is the registration of
   method.**

The methods use nothing from the answer key, so they apply unchanged to a third-party
project.

## A14 — L1: structural correspondence

Each unit of the original is checked for a counterpart in the migrated code.

| Unit | Extracted from the original                                   |
| ---- | ------------------------------------------------------------- |
| Form | each form                                                     |
| Code | each event handler and each procedure or function, per form   |
| Data | each table and column read or written by an SQL statement     |

- Each unit is classified as **matched / missing / added.** Counts are reported per unit
  type.
- A unit that was renamed but has the same role, with a unique counterpart, is
  *matched*. The basis for each such match is recorded.
- Matching by name is checked by role. Where a name match points to the wrong
  counterpart, the correction is recorded.
- Structural quality is not evaluated. L1 records correspondence only.

## A15 — L2: behavioral equivalence

**Purpose.** To identify, independently of the modernizer, every behavioral difference
between the original and the migrated code, and to classify each as declared or
undeclared. §3.3.4 requires the Step 2 report to record every place where behavior
differs; L2 tests whether that record is complete.

**Method: static comparison.** For each procedure of the original, its counterpart is
taken from the L1 correspondence. Four kinds of element are extracted mechanically from
both and compared:

1. **SQL statements** — literal and parameter values masked, compared as token
   sequences; tables, columns, conditions, joins, ordering and aggregation
2. **User-facing text** — strings passed to message boxes; messages and exception texts
   that exist only in the migrated code; on-screen labels and headings
3. **Branch conditions** — the condition of every `If` / `if`, normalised for differences
   of language syntax only (`And`/`&&`, `=`/`==`, `Nothing`/`null`, type conversion
   calls, variable-name prefixes)
4. **Initial values** — values placed in input fields by form-load and field-clearing
   procedures

Where a difference remains after extraction and normalisation, the evaluator reads the
original and the migrated code for that procedure and classifies it by hand. Counts are
per procedure.

**Classification.**

| Category       | Meaning                                                                                       |
| -------------- | --------------------------------------------------------------------------------------------- |
| Same           | no difference                                                                                 |
| Declared       | differs, and the difference is stated in the Step 2 report or in a code comment              |
| **Undeclared** | differs, and the difference is stated nowhere — *silently changed* as defined in §3.3.5      |
| Notation only  | differs in form with no effect on behavior (parameterisation, whitespace, letter case); counted only |

- Results are reported as counts per category for each element kind, and a full list of
  every undeclared difference.
- An element that shows no difference after mechanical extraction is counted as *same*.
- The modernizer's own tests — their number and their results — are **not used** in L2.
- L2 requires a Step 2 report. §6 baseline runs therefore follow the two-step procedure of
  §3.3.4.

**v1 only.** L3 was scored before L2. Where an undeclared difference bears on an L3 item,
that L3 item is reviewed and the review is recorded in the L3 score sheet.

**Limitation.** Static comparison cannot detect differences that appear only at run time:
database type conversion, collation, rendering detail, timing of concurrent operations.
Differences may also be hidden where code is written in a form the extraction rules do not
capture (for example, an SQL statement assembled across several variables). This is stated
with every L2 result.

**Execution-based checking is not performed in v1.** Running the business workflow on
both the original and the migrated application and comparing database state is recorded
as *not performed*. If it is added for §6, its procedure is registered in a further
amendment before any baseline run.

## A16 — The L2 method was changed before judging began

The first proposal for L2 was to execute the extracted calculation code of both versions
on identical inputs and compare outputs. It was rejected, before any L2 judgment, for
three reasons:

1. In applications of this kind, behavior lives mainly in screen event handlers and SQL.
   Calculation code is a small part of it.
2. What can be called in isolation depends on how each modernizer restructured the code,
   so the same measure could not be applied to both outputs.
3. Both modernizers had already compared their calculation code extensively in their own
   tests, so little new information would result.

The change was prompted by the operator questioning whether calculation code alone could
support a claim of equivalence. The method in A15 replaced it.

## A17 — Output size (§3.4, item 6)

§3.4 registers "size of the output — files and lines" without defining what is counted.
The following was decided after the outputs were examined:

- **The primary figure is the migrated application itself:** source files of the
  application (`.vb` / `.cs`), including designer-generated form layout files, which are
  also given as a sub-figure.
- Test code, tools the modernizer wrote during the run, comparison data, and reports are
  each reported separately and are not added to the primary figure.
- **Lines are counted as newline characters.** Blank lines and comment lines are
  included. Build output (`bin`, `obj`), published binaries and images are excluded.
- The original application is counted by the same rule.

A single total is not reported, because comparison data produced by a modernizer can
outweigh the application itself by an order of magnitude and would misrepresent its size.

During the v1 runs the operator also recorded a quick count at freeze time, using a
different rule. It is not used. Where it appears in the run notes, it is marked as such.

This applies to the v1 runs and to the §6 baseline.
