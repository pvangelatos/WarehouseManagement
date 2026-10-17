# Warehouse Management System

Σύστημα διαχείρισης αποθήκης και αποθεμάτων, χτισμένο σε **.NET 10** με **Clean Architecture** και **Domain-Driven Design**.

## Στόχοι
- Παρακολούθηση αποθέματος ανά αποθήκη και θέση (location/bin)
- Πλήρες, αμετάβλητο ιστορικό κινήσεων (παραλαβή, χορήγηση, μεταφορά, διόρθωση)
- Business rules από πραγματική εμπειρία στον στρατιωτικό εφοδιασμό

## Αρχιτεκτονική
| Layer | Ευθύνη |
|---|---|
| Domain | Entities, value objects, aggregates, business rules |
| Application | Use cases, CQRS, validation |
| Infrastructure | EF Core, SQL Server, εξωτερικές υπηρεσίες |
| Api | ASP.NET Core Web API |

Οι εξαρτήσεις δείχνουν προς τα μέσα: το Domain δεν εξαρτάται από κανένα άλλο layer.

## Τεχνολογίες
.NET 10 · C# · xUnit · (σύντομα) EF Core, SQL Server, Docker

## Εκτέλεση
```bash
dotnet build
dotnet test
```

## Κατάσταση
Σε ενεργή ανάπτυξη. Δες το [PROGRESS.md](PROGRESS.md).