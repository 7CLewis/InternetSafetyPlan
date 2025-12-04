using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.UltimateGoalAggregate;

public class UltimateGoal : AggregateRoot
{
    public const int GoalCapacity = 10000;

    public Guid TeamId { get; private set; }
    public UltimateGoalName Name { get; private set; }
    public Description? Description { get; private set; }
    public List<Goal> Goals { get; private set; }

    private UltimateGoal(Guid id, Guid teamId, UltimateGoalName name, Description? description = null)
        : base(id)
    {
        TeamId = teamId;
        Name = name;
        Description = description;
        Goals = new List<Goal>();
    }

    public static Result<UltimateGoal> Create(Guid id, Guid teamId, UltimateGoalName name, Description? description = null)
    {
        var ultimateGoal = new UltimateGoal(id, teamId, name, description);

        return ultimateGoal;
    }

    public Result<UltimateGoal> UpdateInformation(UltimateGoalName name, Description? description = null)
    {
        Name = name;
        Description = description;

        return this;
    }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private UltimateGoal() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
