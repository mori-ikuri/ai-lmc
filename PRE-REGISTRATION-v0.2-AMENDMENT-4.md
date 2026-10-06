# AI-LMC — Pre-registration v0.2, Amendment 4

| | |
|---|---|
| Date | 2026-10-06 |
| Applies to | `PRE-REGISTRATION-v0.2.md` and Amendments 1–3 (all retained unmodified) |
| Status | Registered **before the §6 search began.** No project has been examined under §6. |

## Disclosure

§6 selects "the first project that satisfies all" seven criteria, but registers no order in
which projects are examined. Without one, the choice of where to search, which terms to
use and how to sort the results would decide which project comes first, and that choice
could be made after seeing candidates. This is the freedom A6 was meant to remove.

This amendment fixes the search before it starts. The seven criteria of §6 are not
changed.

§6 also mentions a search conducted during fixture generation. That search is not part of
this procedure, and nothing in the order defined below depends on it.

## A18 — Source and queries

**Source: GitHub only.** Other hosts (SourceForge, archives of CodePlex, and others) are
not searched. GitHub is the only source on which the search can be specified exactly and
repeated. This is a limitation of the search, stated with its result.

Let **D** be the date the search starts, recorded in the search record. Let **C** be the
date three years before D.

The GitHub repository search API is queried with the following four queries, each with
`fork:false` (the default) and sorted by stars, descending:

| # | Query |
|---|---|
| Q1 | `language:"Visual Basic .NET" pushed:<C` |
| Q2 | `language:C# winforms in:name,description,readme pushed:<C` |
| Q3 | `language:C# "windows forms" in:name,description,readme pushed:<C` |
| Q4 | `language:C# topic:winforms pushed:<C` |

- Q1 has no keyword because Visual Basic .NET repositories are predominantly desktop
  applications. C# is used far more widely, so Q2–Q4 restrict it to Windows Forms.
- `pushed:<C` is a pre-filter for criterion 6. A repository whose default branch has had
  no commit since C, but which received a push to another branch after C, is excluded by
  it. This is disclosed with the result.
- The API returns at most 1,000 results per query. Results beyond that are not examined.
- C# repositories that mention Windows Forms only in another language, or not at all, are
  not found. This is disclosed with the result.

The raw responses of all four queries are saved, with the time of retrieval.

## A19 — Order of examination

The results of Q1–Q4 are merged and de-duplicated. The merged list is sorted by **stars,
descending**, then by **full repository name, ascending** (`owner/name`, ordinal). This
sorted list, taken from the saved responses, is the order of examination. It is published
with the search record.

The list is not re-queried or re-sorted during the search.

## A20 — How each criterion is judged

**The application.** Criterion 1 identifies the application: the project in the repository
with output type `WinExe` that contains the most classes inheriting
`System.Windows.Forms.Form`, together with the projects in the same repository that it
references. Criteria 2–4 are judged on the application.

| # | Criterion (§6) | Judged by |
|---|---|---|
| 1 | Windows Forms desktop application, VB.NET or C# | a `WinExe` project in VB.NET or C# containing at least one class inheriting `System.Windows.Forms.Form`. WPF-only or console-only fails |
| 2 | targets .NET Framework 4.x | the application project's `TargetFrameworkVersion` is v4.0–v4.8.x, or, for an SDK-style project, its target framework is `net40`–`net48x` |
| 3 | accesses a relational database | the application contains code that connects to a relational database through an ADO.NET provider or an ORM. Where the only provider is OLE DB or ODBC, the connection target is confirmed by reading the code; Excel or text files alone do not count |
| 4 | at least 5,000 lines across at least 8 forms | lines counted by the rule of A17 (newline characters; designer files included; test projects, `bin`, `obj` and package folders excluded). Forms are classes inheriting `Form`, directly or through another form class in the repository; partial classes count once. Directories of third-party code with its own licence are excluded, and listed |
| 5 | permissive licence permitting derivative works | the licence file at the repository root, or, if there is none, the licence stated in the README: MIT, Apache-2.0, BSD-2-Clause or BSD-3-Clause. Anything else, or no licence, fails |
| 6 | no commit in the last three years | the most recent commit on the default branch has a committer date before C |
| 7 | no published AI-assisted migration | a web search for `"owner/name"`, `"name" migration .NET` and `"name" AI migration`, and a GitHub repository search for `name`. It fails only if a public source states that the project was migrated with AI assistance. Migrations without AI assistance are recorded and do not fail it |

Criteria are applied in the order 1–7. Examination of a project **stops at the first
criterion it fails**, and that criterion is recorded with its basis. Mechanical checks are
done by script, and the scripts and their output are saved. Where a judgment is made by
hand, the reason is recorded.

A repository that cannot be retrieved (deleted, disabled or empty) is recorded as
unavailable and skipped.

## A21 — Stopping, and the record

- The first project that passes all seven criteria is selected, and the search stops.
  Projects after it are not examined.
- The search record lists every project examined, the criterion it failed and the basis,
  and the selected project with the evidence for each criterion.
- If the list is exhausted with no project selected, the baseline is recorded as **not
  performed** under §6, with the search record as the reason. Adding a source or a query
  would require a further amendment, registered before any candidate from it is
  examined.
- Facts noticed about a project that are not criteria — for example, that the original
  does not build, or that it depends on a commercial component — are recorded as
  observations. They do not exclude the project.

## A22 — Other decisions for the §6 baseline

- **Execution-based checking is not added for §6.** L2 uses the static comparison of A15
  only, as in v1.
- **The hand-over and the request for the baseline runs** (what is given to each
  modernizer, and the wording of the request) are registered in a further amendment
  before any baseline run.
- A widely starred project is more likely to appear in the training data of both
  modernizers. This cannot be controlled, and is stated with the baseline result.
