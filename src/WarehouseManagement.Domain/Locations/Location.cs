using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.Locations;

public sealed class Location
{
    public Guid Id { get; }
    public LocationCode Code { get; }
    public LocationStatus Status { get; private set; } = LocationStatus.Active;

    private Location(Guid id, LocationCode code)
    {
        Id = id;
        Code = code;
    }

    public static Location Create(LocationCode code)
    {
        ArgumentNullException.ThrowIfNull(code);

        Guid id = Guid.CreateVersion7();

        return new Location(id, code);
    }

    public void Deactivate(bool isEmpty)
    {
        if (Status is LocationStatus.Inactive)
            throw new BusinessRuleException($"This location {Code} is already inactive");

        if (!isEmpty)
            throw new BusinessRuleException($"Location {Code} is not empty. Transfer its stock to another location before deactivating it");

        Status = LocationStatus.Inactive;
    }
}
