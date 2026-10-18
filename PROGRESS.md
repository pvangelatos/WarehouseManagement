# Progress

## Τρέχουσα φάση
**Φάση 1 — Θεμέλια** (solution structure, Git, σύγχρονη C#, domain modeling)

## Ολοκληρωμένα tasks
- [x] 1.1 Δημιουργία repo, .gitignore
- [x] 1.1 README / PROGRESS
- [x] 1.2 Solution structure (src/ & tests/, project references)
- [x] 1.3 Directory.Build.props, .editorconfig, global.json
- [x] 1.4 docs/domain.md (Ubiquitous Language, business rules)

## Αποφάσεις
| Θέμα | Απόφαση | Αιτιολόγηση |
|---|---|---|
| Αρχιτεκτονική | Clean Architecture + DDD | Σαφή όρια layers, συνηθισμένη σε .NET θέσεις εργασίας |
| Framework | .NET 10 | LTS έκδοση |
| Solution format | `.slnx` | Νέο default, πιο ευανάγνωστο XML |
| Δομή φακέλων | `src/`, `tests/` | Διαχωρισμός παραγωγικού κώδικα και tests |
| Git (Φάση 1) | Commits στο main, Conventional Commits | Απλότητα στο ξεκίνημα |
| Git (Φάση 2+) | Branch ανά task + PR | Προσομοίωση εταιρικής ροής |
| Build settings | `Directory.Build.props`, `TreatWarningsAsErrors`, `EnforceCodeStyleInBuild` | Κοινές ρυθμίσεις σε ένα σημείο, code style ελέγχεται στο build και όχι μόνο στον editor |
| Ποσότητες κινήσεων | Πάντα θετικές, η κατεύθυνση ορίζεται από τον τύπο κίνησης | Αποφεύγονται bugs με πρόσημα, ο τύπος έχει business νόημα |
| Business rules | Τεκμηριωμένα στο `docs/domain.md` με σταθερούς κωδικούς BR-xx | Τα tests θα αναφέρονται στους κωδικούς, γι' αυτό δεν γίνεται επαναρίθμηση |
| Διόρθωση λαθών κίνησης | Μόνο με Reversal, ποτέ με Adjustment (BR-15a) | Ένας τρόπος διόρθωσης, καθαρό audit trail |
| Έγκριση Adjustment | Μόνο οι μειώσεις άνω του 20% απαιτούν έγκριση (BR-14) | Ο κίνδυνος είναι κυρίως η απόκρυψη απωλειών |

## Επόμενο βήμα
Ερωτήσεις συνέντευξης Φάσης 1 → έναρξη Φάσης 2 (Domain layer)
