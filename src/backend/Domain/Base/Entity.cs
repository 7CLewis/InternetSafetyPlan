namespace InternetSafetyPlan.Domain.Base;

public abstract class Entity
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; }
    public DateTime LastUpdated { get; set; }
    public bool IsDeleted { get; set; } = false;

    protected Entity(Guid id)
    {
        Id = id;
        Created = DateTime.Now;
        LastUpdated = DateTime.Now;
    }

    public void Delete() => IsDeleted = true;
    public void UndoDeletion() => IsDeleted = false;

    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo))
            return true;

        if (ReferenceEquals(null, compareTo))
            return false;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * 907) + Id.GetHashCode();
    }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
    protected Entity() { }
}
