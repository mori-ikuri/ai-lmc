# AI-LMC — Pre-registration v0.2, Amendment 2

| | |
|---|---|
| Date | 2026-10-05 |
| Applies to | `PRE-REGISTRATION-v0.2.md` and Amendment 1 (both retained unmodified) |
| Status | Registered after both runs were completed and frozen, **before any scoring**. |

## Disclosure

Both runs finished on 2026-10-04 and their outputs are frozen in a private repository.
The post-run log check required by A11 was carried out on 2026-10-05. While reading the
CROSS session log, the operator saw the tool's final summary message (build result and
the number of tests it reported). No other part of either output had been examined when
this amendment was written.

## A12 — CROSS run001 is a contamination event under A11

A11 registers that any network access to anything other than the model provider's API and
package sources is reported, and that the run is handled as a contamination event under §8.

The CROSS session log (Codex CLI, 2026-10-04) records the following, all via shell
commands; the web tools disabled under A9 were not used:

- 03:00:11Z–03:00:35Z: requests to `raw.githubusercontent.com` and `api.github.com` to read
  the source of ICSharpCode.CodeConverter, a package it had restored from NuGet
- 03:11:11Z: a request to `www.microsoft.com` (a SQL Server download page)
- 03:11:23Z–03:12:09Z: requests to `api.github.com` for SQL Server entries in
  `microsoft/winget-pkgs`
- 03:12:38Z: an attempt to download a SQL Server LocalDB installer from
  `download.microsoft.com`. The tool's own approval review rejected the command before it
  executed. Nothing was downloaded or installed.

The log contains no reference to this repository or its owner's account, and no use of
web search. The accessed content appears unrelated to the answer key. **This does not
change the handling.** A11 does not condition the rule on what was accessed, and the rule
is applied as registered.

Handling:

1. CROSS run001 is **excluded from the primary analysis.** The SELF/CROSS comparison is
   not reported as a registered result.
2. CROSS run001 may be scored. Any such result is labelled **exploratory, not
   pre-registered** wherever it appears, together with this amendment.
3. Whether to conduct a further CROSS run is decided separately. If one is conducted, its
   conditions — including how non-package network access is prevented — are registered in
   a further amendment before it starts.

The SELF session log was checked in the same way. Its only network access outside the
provider API was to the NuGet package source. SELF run001 is not affected.

## A13 — SELF received the fixed reply as two messages

The fixed reply in §3.3.4 was entered in two parts, at 01:37:46Z and 01:37:50Z, because
of a paste error. Together the two messages are identical to the registered Japanese
text. The second part was delivered while the tool was already working on the first.

This is counted and reported as **one intervention** under §3.3.5, with both messages and
timestamps given in full in the results.

CROSS received the fixed reply as a single message, identical to the registered text.
