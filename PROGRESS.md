# Progress

## Τρέχουσα φάση
**Φάση 2 — Domain layer (entities, value objects, aggregates, invariants, xUnit tests)**

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
- [x] 2.3a docs/domain.md: κύκλος ζωής Item, BR-26..30
- [x] 2.3b Entity Item
- [x] 2.3c-1 Item.Discontinue (BR-27) + tests 
- [x] 2.3c-2 — Item.ChangeReorderPoint (BR-30)
- [x] 2.3c-3 — Item.Deactivate (BR-27, BR-29) + tests
- [x] 2.3d — PR feature/2.3-item → main
- [x] 2.3e Test helper CreateItem (refactor ItemTests)
- [x] 2.4a docs/domain.md: θέσεις αποθήκευσης (BR-18 αναθεώρηση, BR-31..35)
- [x] 2.4b Value object LocationCode (Create, Parse, normalization) + tests (BR-31)
- [x] 2.4c Entity Location (Create, Deactivate, Activate) + tests (BR-18, BR-32..34)
- [x] 2.4d PR feature/2.4-location → main
- [x] 2.5a docs/domain.md: κινήσεις αποθέματος (BR-36..44), αναθεώρηση BR-15, BR-28
- [x] 2.5b Enums MovementType, MovementDirection, AdjustmentReason + extensions + tests (BR-15, BR-36)
- [x] 2.5c-1 Value object SupplierDocument + tests (BR-39)
- [x] 2.5c-2 Entity StockMovement + CreateReceipt (guards, BR-02, BR-38, BR-39, BR-44) + tests
- [x] 2.5c-3a StockMovement.CreateIssue + test helper CreateIssue + tests (BR-02, BR-28, BR-38, BR-44)

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
| Όνομα υλικού | `string` με έλεγχο μέσα στο `Item` (όχι value object) | Χρησιμοποιείται μόνο από το `Item`, χωρίς πράξεις· YAGNI |
| Σειρά ελέγχων σε methods | Πρώτα οι προϋποθέσεις κατάστασης, μετά η εγκυρότητα των δεδομένων, και αλλαγή κατάστασης μόνο αφού περάσουν όλοι οι έλεγχοι | Ο χρήστης βλέπει πρώτα το σφάλμα που ακυρώνει την ενέργεια· μετά από exception το entity μένει αμετάβλητο (always-valid) |
| Έλεγχος αποθέματος στο Deactivate (BR-29) | Το απόθεμα περνά ως παράμετρος, `Deactivate(Quantity currentStock)`. Μετάβαση σε domain service με `internal` method όταν υπάρξει το aggregate αποθέματος | Ο κανόνας ζει και ελέγχεται στο Domain από τώρα· το aggregate αποθέματος δεν υπάρχει ακόμα (YAGNI). Το ρίσκο λάθους τιμής από τον caller καλύπτεται με tests στο Application |
| Δημιουργία Item στα tests | Helper `CreateItem` με optional parameters για το Arrange· τα tests του `Create` καλούν απευθείας `Item.Create` | Κάθε test δείχνει μόνο τις τιμές που επηρεάζουν το σενάριο· ο helper δεν κρύβει τη method που ελέγχεται |
| Κωδικός θέσης | Value object με `Aisle`, `Bay`, `Level` ως ξεχωριστά properties, ο κωδικός προκύπτει από αυτά | Η δομή είναι μέρος του ορισμού της θέσης· ομαδοποίηση και φιλτράρισμα ανά διάδρομο ή επίπεδο (απογραφή, συλλογή) χωρίς string parsing, απλός έλεγχος ορίων με αριθμούς |
| Όρια κωδικού θέσης | Όρια μορφής (3-2-2 ψηφία) και όχι φυσικής διάταξης | Νέος διάδρομος δεν απαιτεί αλλαγή κώδικα· ποιες θέσεις υπάρχουν ορίζεται από τις εγγραφές. Τα φυσικά όρια, αν χρειαστούν, θα είναι ρύθμιση |
| Κύκλος ζωής θέσης | Ενεργή ⇄ Ανενεργή, αναστρέψιμος (σε αντίθεση με το `Item`) | Το ράφι επισκευάζεται και ξαναμπαίνει σε χρήση· η αντικατάσταση στο ίδιο σημείο επανενεργοποιεί την υπάρχουσα θέση, ώστε το ιστορικό να μη χωρίζεται |
| Ένα υλικό ανά θέση (BR-35) | Ελέγχεται στις κινήσεις και όχι στο `Location` | Η θέση δεν γνωρίζει το απόθεμά της· ο κανόνας ανήκει εκεί όπου αλλάζει το απόθεμα |
| Έλεγχος BR-18 | `Deactivate(bool isEmpty)` | Η θέση χρειάζεται μόνο ναι/όχι. Το `Quantity` δεν έχει νόημα για θέση, γιατί το άθροισμα διαφορετικών υλικών (π.χ. ζεύγη + κιλά) δεν έχει μονάδα. Μετάβαση σε domain service μαζί με το `Item.Deactivate` |
| Δημιουργία LocationCode | Create(int, int, int) για έλεγχο ορίων, Parse(string) για μορφή, και η Parse καλεί την Create | Ένα μόνο σημείο ελέγχου ορίων· το regex ελέγχει μορφή και η Create τιμή |
| Ψηφία σε regex | [0-9] και όχι \d | Στο .NET το \d ταιριάζει κάθε ψηφίο Unicode (π.χ. αραβικά, fullwidth), ενώ το int.Parse δέχεται μόνο ASCII· η είσοδος θα περνούσε τον έλεγχο μορφής και θα έσκαγε με FormatException |
| ToString σε value objects | Override που επιστρέφει την κανονική μορφή | Το default ToString του record τυπώνει όλα τα properties· χρησιμοποιείται σιωπηρά σε interpolation, logs και μηνύματα |
| Είσοδος του Location.Create | Δέχεται `LocationCode` και όχι string ή αριθμούς | Το `LocationCode` έχει δύο εισόδους (Create, Parse)· με private constructor κάθε instance είναι ήδη έγκυρο, οπότε το entity δεν επαναλαμβάνει ελέγχους ούτε διπλασιάζει το API. Διαφέρει από το `Item.Create(string sku)` γιατί το `Sku` έχει μία μόνο είσοδο |
| Κατάσταση θέσης | `enum LocationStatus` (Active = 1, Inactive = 2) και όχι `bool` | Καθαρά μηνύματα λάθους, συνέπεια με το `ItemStatus`, επέκταση χωρίς αλλαγή τύπου. Τα enums ξεκινούν από 1 ώστε το default 0 να σημαίνει «δεν ορίστηκε» |
| Απογραφή και θέση (BR-16) | Δεν είναι κατάσταση του `Location`· θα ελέγχεται από το μελλοντικό aggregate `StockCount` | Η πληροφορία ανήκει στην απογραφή· αποφεύγονται συγκρουόμενες καταστάσεις και δύο σημεία αλήθειας |
| Ένα σενάριο ανά test | Σε κάθε test μόνο ένας λόγος αποτυχίας· η σειρά ελέγχων ελέγχεται από ξεχωριστό test | Αλλιώς ένα test περνά για λάθος λόγο και δεν πιάνει την αφαίρεση ενός κανόνα |
| Υπόλοιπο αποθέματος | Ledger κινήσεων + aggregate `StockLevel` με το τρέχον υπόλοιπο, ενημέρωση στο ίδιο transaction | Το ledger δίνει πλήρες ιστορικό (BR-04), το υπόλοιπο γρήγορο έλεγχο BR-01. Invariant: υπόλοιπο = άθροισμα κινήσεων |
| Σχήμα κίνησης | Κάθε κίνηση αφορά ένα υλικό και μία θέση | Ομοιόμορφο σχήμα, χωρίς nullable θέσεις· το υπόλοιπο θέσης είναι απλό άθροισμα |
| Μεταφορά | Δύο κινήσεις (Transfer Out / In) με κοινό αναγνωριστικό, πάντα μαζί | Λογική διπλογραφίας: ένα γεγονός για τον χρήστη, χρέωση και πίστωση στο ledger |
| Κατεύθυνση κίνησης | Μόνο από τον τύπο (AdjustmentIncrease / AdjustmentDecrease, όχι πεδίο Direction) | Ένας κανόνας χωρίς εξαιρέσεις (BR-02)· οι μειώσεις του BR-14 εντοπίζονται από τον τύπο |
| Αιτιολογίες Adjustment | Κάθε αιτιολογία δηλώνει σε ποιες κατευθύνσεις επιτρέπεται | Ίδιο μοτίβο με το `UnitOfMeasure`· νέα αιτιολογία χωρίς αλλαγή σε άλλο σημείο |
| Reversal | Ολόκληρη η κίνηση, υποχρεωτική αιτιολογία, δεν αντιστρέφεται, η μεταφορά μόνο ολόκληρη | Ένας τρόπος διόρθωσης (BR-15a), χωρίς αλυσίδες· ο ελεγκτής βλέπει ποιος και γιατί |
| Μοναδικότητα αντιλογισμού (BR-05) | `ReversedMovementId` στον αντιλογισμό, έλεγχος στο Application + unique index στη βάση | Η αρχική κίνηση μένει αμετάβλητη· το index προστατεύει από ταυτόχρονες καταχωρήσεις |
| Ημερομηνίες κίνησης | Ημερομηνία παραστατικού (business, χωρίς όριο προς τα πίσω) + ημερομηνία καταχώρησης (αυτόματη, για έλεγχο) | Αναφορές με την πραγματική ημερομηνία, εντοπισμός αναδρομικών καταχωρήσεων |
| Adjustment με έγκριση | Η κίνηση δημιουργείται μόνο μετά την έγκριση | Το ledger καταγράφει μόνο ό,τι έγινε πραγματικά· το αίτημα είναι ξεχωριστή έννοια |
| Exceptions για λάθη προγραμματιστή | Built-in του .NET: `InvalidOperationException` (η κλήση δεν έχει νόημα για το αντικείμενο), `ArgumentException` (ακατάλληλο όρισμα), `ArgumentOutOfRangeException` (τιμή εκτός εύρους). `BusinessRuleException` μόνο για παραβιάσεις κανόνων από τον χρήστη | Τα λάθη χρήστη γίνονται 4xx με μήνυμα, τα bugs γίνονται 500 και καταγράφονται |
| Κατεύθυνση του Reversal | Το `Direction()` πετά `InvalidOperationException` για τον Reversal· η κατεύθυνση υπολογίζεται από την κίνηση, ως αντίθετη της αρχικής | Ο τύπος από μόνος του δεν έχει την πληροφορία (Information Expert) |
| Πληρότητα κανόνων σε enums | Test με `Enum.GetValues` + `[MemberData]` που αποτυγχάνει όταν μια νέα τιμή δεν έχει κανόνα | Μια ξεχασμένη περίπτωση στο switch πιάνεται στο build και όχι στην παραγωγή |
| Assert σε exceptions του .NET | Έλεγχος `ParamName` / `ActualValue`, όχι `Message` | Η μορφή του `Message` ορίζεται από το .NET και μπορεί να αλλάξει |
| Πεδία ανά τύπο κίνησης | Nullable πεδία (π.χ. `SupplierDocument?`) + named factories, όχι κληρονομικότητα | Η factory εγγυάται τα σωστά πεδία ανά τύπο· ένας πίνακας στη βάση. Επανεξέταση αν οι τύποι αποκτήσουν διαφορετική συμπεριφορά |
| Ποσότητα στις factories κίνησης | Δέχονται `decimal` και φτιάχνουν το `Quantity` με τη μονάδα του υλικού | Οι BR-22, 23, 25 εγγυώνται στο Domain· ο caller δεν μπορεί να περάσει λάθος μονάδα |
| Τύποι ημερομηνιών κίνησης | `DateOnly` για την ημερομηνία παραστατικού, `DateTimeOffset` (παράμετρος) για την καταχώρηση | Το παραστατικό δεν έχει ώρα· το Domain δεν διαβάζει ρολόι, το offset κρατά την τοπική ώρα |
| Χρήστης κίνησης | `string RecordedBy`, έλεγχος με `ArgumentException.ThrowIfNullOrWhiteSpace` | Το γεμίζει το σύστημα από το login, άρα κενό σημαίνει bug και όχι λάθος χρήστη. Αλλαγή μετά τη Φάση 6 (JWT) |
| Αριθμός παραστατικού | `string`, οποιοσδήποτε χαρακτήρας, έως 30, κεφαλαία με `ToUpperInvariant` | Αντιγράφουμε το εξωτερικό έγγραφο (ελληνικές σειρές, μηδενικά μπροστά)· το invariant δίνει ίδιο αποτέλεσμα σε κάθε server |
| Σειρά ελέγχων σε factories | Πρώτα guard clauses ορισμάτων, μετά κατάσταση, μετά δεδομένα | Δεν ελέγχεται η κατάσταση ενός αντικειμένου που είναι null |
| Ημερομηνία καταχώρησης για τον BR-38 | `DateOnly.FromDateTime(recordedAt.DateTime)` και όχι `UtcDateTime` | Η ημερομηνία όπως τη ζει ο χρήστης στη ζώνη του· με UTC ένα τιμολόγιο της ίδιας μέρας θα απορριπτόταν μετά τα μεσάνυχτα |
| Ημερομηνίες σε μηνύματα | Ρητό format `yyyy-MM-dd` | Το default format εξαρτάται από τη γλώσσα του υπολογιστή· τα tests θα έσπαγαν σε άλλο περιβάλλον (CI) |
| Helper `CreateReceipt` στα tests | Optional parameters με `??` για τα αντικείμενα· τα tests των null guards καλούν απευθείας τη factory | Κάθε test δείχνει μόνο ό,τι αλλάζει· ο helper δεν μπορεί να ξεχωρίσει «δεν δόθηκε» από «δόθηκε null» |
| Χορήγηση και Issue Request (BR-10, BR-11) | Η `CreateIssue` δεν δέχεται ακόμα Issue Request· ο έλεγχος θα προστεθεί με το aggregate `IssueRequest` | Το aggregate δεν υπάρχει ακόμα (YAGNI)· χωρίς nullable πεδίο «για μελλοντική χρήση» |
| Αρνητικό απόθεμα (BR-01) στη χορήγηση | Δεν ελέγχεται στη `StockMovement` | Η κίνηση δεν γνωρίζει το υπόλοιπο της θέσης· ο κανόνας ανήκει εκεί όπου υπολογίζεται το απόθεμα |
| Σειρά υλοποίησης: πρώτα 2ο αντίγραφο, μετά refactoring | Η `CreateIssue` γράφτηκε με tests πριν από την εξαγωγή κοινών ελέγχων | Με δύο αντίγραφα φαίνεται τι είναι πραγματικά κοινό· τα tests αποδεικνύουν ότι το refactoring δεν αλλάζει συμπεριφορά |

## Επόμενο βήμα
Task 2.5c-3b: refactoring κοινών ελέγχων της StockMovement σε private methods (θέση, ποσότητα, ημερομηνία) + απόφαση α/β για τον έλεγχο κατάστασης υλικού (BR-44). Τα tests δεν αλλάζουν.
Μετά: 2.5d Adjustment + Transfer, 2.5e Reversal, 2.5f PR.
