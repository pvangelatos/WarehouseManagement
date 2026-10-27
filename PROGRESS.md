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
- [x] 2.2a docs/domain.md: ποσότητες και μονάδες μέτρησης (BR-21 έως BR-25)
- [x] 2.2b Enum UnitOfMeasure με extensions (AllowedDecimals, UnEceCode) + tests (BR-24)
- [x] 2.2c Value object Quantity (validation BR-21..23, operators, Zero) + tests 

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
| Έλεγχος δεκαδικών | `decimal.Round(value, n) != value` και όχι `decimal.Scale` | Το `Scale` μετρά και τα trailing zeros (1.2300 = 4), άρα θα απέρριπτε έγκυρες τιμές |
| Operators του Quantity | Χωρίς μονάδα, μέσω private constructor | Άθροισμα/διαφορά έγκυρων ποσοτήτων δεν αυξάνει τα δεκαδικά. Η συμβατότητα μονάδων εξασφαλίζεται από το `Item` (BR-25) |
| Test data με decimal | `TheoryData` + `[MemberData]` | Τα attributes δεν δέχονται `decimal`, και το `double` χάνει τα trailing zeros |
| Ταυτότητα Item | Surrogate key (`Guid`, v7) + SKU ως μοναδικός business identifier | Το Id δεν εξαρτάται από business δεδομένα· οι χρήστες ψάχνουν με SKU, το σύστημα συνδέει με Id |
| Entities | `class`, όχι `record` | Ισότητα κατά ταυτότητα· το `with` θα δημιουργούσε αντίγραφα της ίδιας entity |
| Κύκλος ζωής Item | Active → Discontinued → Inactive, μόνο προς τα εμπρός | Αντικατοπτρίζει την πραγματική απόσυρση υλικού: πρώτα εξάντληση υπολοίπου, μετά κλείσιμο |
| Κανόνες εκτός Item | Μοναδικότητα SKU και ιστορικό κινήσεων (BR-19) ελέγχονται στο Application layer | Το Item δεν γνωρίζει άλλα υλικά ούτε τις κινήσεις του |

## Επόμενο βήμα
Task 2.3a — BR-26..30 στο domain.md (αποφάσεις κλειδωμένες, μένει η διατύπωση) → 2.3b Entity Item
