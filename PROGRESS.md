# Progress

## Τρέχουσα φάση
**Φάση 2 — Domain layer (entities, value objects, aggregates, invariants, xUnit tests)

## Ολοκληρωμένα tasks
- [x] 1.1 Δημιουργία repo, .gitignore
- [x] 1.1 README / PROGRESS
- [x] 1.2 Solution structure (src/ & tests/, project references)
- [x] 1.3 Directory.Build.props, .editorconfig, global.json
- [x] 1.4 docs/domain.md (Ubiquitous Language, business rules)
- [x] Ερωτήσεις συνέντευξης Φάσης 1
- [x] 2.1 Value object SKU + BusinessRuleException + unit tests (BR-20)

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
| Merge strategy | Squash merge only, ruleset protect-main (PR υποχρεωτικό, χωρίς force push) | Καθαρό ιστορικό στο main, ένα commit ανά task |
| Domain errors | Throw `BusinessRuleException` στο Domain, Result pattern στο Application (Φάση 3) | Το Domain είναι η τελευταία γραμμή άμυνας, το Application κάνει validation πριν φτάσει εκεί |
| Branch naming | `feature/<task-id>-<περιγραφή>` (π.χ. `feature/2.1-sku`) | Το branch δείχνει αμέσως σε ποιο task ανήκει |
| Οργάνωση Domain | Φάκελοι ανά έννοια του domain (`Items/`, `Exceptions/`) και όχι ανά τεχνικό τύπο | Ό,τι αφορά μια έννοια βρίσκεται μαζί, ακολουθεί το Ubiquitous Language |
| Μορφή SKU | Μόνο λατινικοί χαρακτήρες A-Z, ψηφία και παύλα (BR-20) | Αποφυγή διπλών υλικών από ελληνικούς/κυριλλικούς χαρακτήρες που μοιάζουν ίδιοι |
| Τύπος ποσότητας | `decimal`, μέχρι 3 δεκαδικά | Ακρίβεια χωρίς σφάλματα floating point, 3 δεκαδικά καλύπτουν γραμμάρια/χιλιοστόλιτρα/χιλιοστά |
| Μηδενική ποσότητα | Ένας τύπος `Quantity` (≥ 0), το «> 0» ελέγχεται στην κίνηση | Ένα μόνο σημείο χρειάζεται «> 0» (YAGNI). Refactor σε `PositiveQuantity` αν εμφανιστεί δεύτερο |
| Unit of Measure | Smart enum value object, ανήκει στο `Item` και όχι στο `Quantity` | Κλειστή λίστα χωρίς παραλλαγές γραφής, κάθε μονάδα ξέρει αν επιτρέπει δεκαδικά. Όλες οι ποσότητες στη βασική μονάδα του υλικού, οπότε δεν προστίθενται κιλά με τεμάχια |

## Επόμενο βήμα
Task 2.2 — Quantity & UnitOfMeasure (2.2a domain.md → 2.2b UnitOfMeasure → 2.2c Quantity)
