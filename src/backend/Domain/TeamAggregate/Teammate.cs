using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.TeamAggregate;

public class Teammate : Entity
{
    public Guid TeamId { get; private set; }
    public Guid? UserId { get; private set; }
    public TeammateName Name { get; private set; }
    public DateOfBirth? DateOfBirth { get; private set; }

    private Teammate(Guid id, Guid teamId, TeammateName name, DateOfBirth? dateOfBirth = null, Guid? userId = null)
        : base(id)
    {
        TeamId = teamId;
        UserId = userId;
        Name = name;
        DateOfBirth = dateOfBirth;
    }

    public static Result<Teammate> Create(Guid id, Guid teamId, TeammateName name, DateOfBirth? dateOfBirth = null, Guid? userId = null)
    {
        var teammate = new Teammate(id, teamId, name, dateOfBirth, userId);

        return teammate;
    }

    public Result<Teammate> UpdateInformation(TeammateName name, DateOfBirth? dateOfBirth = null, Guid? userId = null)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        UserId = userId;

        return this;
    }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Teammate() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
