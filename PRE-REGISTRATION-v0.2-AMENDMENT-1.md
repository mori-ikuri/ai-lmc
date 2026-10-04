# AI-LMC — Pre-registration v0.2, Amendment 1

| | |
|---|---|
| Date | 2026-10-04 |
| Applies to | `PRE-REGISTRATION-v0.2.md` (retained unmodified) |
| Status | Registered before the SELF run. No modernization run has started. |

## A8 — Context reachable through the service login is excluded

§8.5 isolates the operating-system account. Pre-run checks found context that reaches
a tool through the service account it is signed in to: connectors configured in the
web account, skills synchronised from the web account, browser-control integrations,
tool-side memory, and built-in tools that read data stored in the account or exchange
messages with other sessions.

Rule, applied identically in both conditions: **anything the tool ships with by default
is kept, except built-in tools that reach account-stored data or other sessions;
anything the operator added or enabled is removed.**

- SELF (Claude Code CLI): auto memory, web-account connectors, browser integration,
  web-account skill sync, account-artifact tools and inter-session messaging tools are
  disabled. Verified before the run.
- CROSS (Codex CLI, not the desktop app): a fresh tool home directory; memories,
  apps/connectors, browser and computer-use plugins, user-installed skills and the
  equivalent built-in tools are not loaded. Verified before the CROSS run, and the
  result is recorded before the task is given.

The exact launch configuration of each condition is published with the results.

## A9 — Web tools are disabled by configuration

Web search and web fetch tools are disabled in the tool configuration in both
conditions. Web search may execute on the provider's side, so it cannot be controlled
by local network settings. See also A11.

## A10 — §8.5 is not performed; replaced by access denial

Runs use the operator's normal OS account. In its place, in addition to A8, read access
for that account is denied to every location holding the answer key, fixture generation
history, the public repository, earlier tool session history, and — for CROSS — the
SELF output.

A rehearsal found that inherited denial did not reach every file, including six files in
the answer-key location. Denial is therefore verified per file, and applied explicitly
wherever inheritance did not reach. For CROSS, denial is verified against the account
under which the tool executes commands. Denial is checked before the task is given and
removed after both runs.

## A11 — §8.7 is not performed as written

Network access is not disabled during the runs. Both tools require the model provider's
API, and building the target may require package restore, so a general network block
would alter the task.

Instead, web search and web fetch are disabled (A9). The modernizer retains shell
access and could in principle reach any address, including this repository. After each
run, the full session log is checked for network access to anything other than the
provider API and package sources, and in particular for any reference to this
repository or its owner's account. Any such access is reported, and the run is handled
as a contamination event under §8.
