# Working rules

These rules cover deliverables and format only. The request itself is in `依頼メモ.md`.

---

## 1. Two steps

Work in two steps. **Stop after Step 1 and wait.**

### Step 1 — Assessment

Do not modify any file under `src/`.

Write `report/01_assessment.md` containing:

1. **Overview and business rules.** What the system does, and the business rules you have
   identified. For each rule, give the source location (file and line) you identified it from.
2. **Migration risks and intended approach**, including how you intend to handle reporting.
3. **Questions.** Things you would want to confirm. For each one:
   - what you want to know
   - why it matters for the migration
   - **what you intend to do if no answer is available**

Then stop. Do not begin Step 2 until you receive a reply.

### Step 2 — Migration

Carry out the migration.

Write `report/02_migration.md` recording **every place where the behavior of the new
program differs from the original**, with the reason for each difference.

## 2. Target

| | |
|---|---|
| Language | C# |
| Framework | .NET 10 (LTS) |
| UI | Windows Forms |
| Database | Schema unchanged. No data migration. |
| Reporting | Your choice. Record why you chose it in `report/02_migration.md`. |

**The build must succeed.**

No new functionality is to be added.

## 3. Layout

```
src/        the existing application — Step 1: read only
new/        the migrated application
report/     01_assessment.md, 02_migration.md
```

Leave `src/` in place. Do not delete or move it.

## 4. Other

- Writing tests is permitted.
- There is no time limit.
- Japanese is fine for the reports; so is English. Use whichever you prefer.
