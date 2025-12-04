namespace InternetSafetyPlan.Domain.Base;

public class AggregateRoot : Entity
{
    public AggregateRoot(Guid id) : base(id) { }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
    protected AggregateRoot() { }
}
