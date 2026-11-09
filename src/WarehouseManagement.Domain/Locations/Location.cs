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
}
