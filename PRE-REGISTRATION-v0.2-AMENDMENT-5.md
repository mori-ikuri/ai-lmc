# AI-LMC — Pre-registration v0.2, Amendment 5

| | |
|---|---|
| Date | 2026-10-06 |
| Applies to | `PRE-REGISTRATION-v0.2.md` and Amendments 1–4 (all retained unmodified) |
| Status | Registered **during the §6 search**, after ranks 1–40 of the order of examination had been examined and before any later rank. No project has been selected. |

## Disclosure

The §6 search started on 2026-10-06 under Amendment 4. The four queries returned 3,668
distinct repositories, and the order of examination was fixed from the saved responses.
Ranks 1–40 were then examined. None was selected.

Rank 1 is a charting library. Under the rule of A20, its sample application counts as "the
application" for criterion 1, and the library code it references counts toward criterion 4.
It passed criteria 1–6 by script and failed criterion 3 only when read by hand (the matched
token was a UI data-binding property, not a database). A library whose sample application
does connect to a database could therefore pass all seven criteria and be selected, although
§6 asks for a Windows desktop application.

This amendment closes that gap before any rank after 40 is examined. No project seen so far
depended on it: it only makes criterion 1 stricter, so no project that failed can now pass.

## A23 — Criterion 1 requires the repository to be an application

In addition to the rule of A20, criterion 1 requires that **the repository's primary
product is the application itself.** It fails if:

- the primary product is a library, control, component or framework, and the `WinExe`
  projects serve as samples, demos or tests of it; or
- the repository is a collection of samples, exercises or examples rather than one
  application.

This is judged by hand from the repository's description and README, and the reason is
recorded.

Ranks 1–40 are judged again under this rule. The search record shows, for each, both the
criterion recorded before this amendment and the criterion under it.
