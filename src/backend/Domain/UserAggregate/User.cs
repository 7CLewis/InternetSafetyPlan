using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.UserAggregate;

public class User : AggregateRoot
{
    public Email Email { get; private set; }
    public Guid? TeamId { get; private set; }

    private User(Guid id, Email email, Guid? teamId = null)
        : base(id)
    {
        Email = email;
        TeamId = teamId;
    }

    public static Result<User> Create(Guid id, Email email, Guid? teamId = null)
    {
        var user = new User(id, email, teamId);

        return user;
    }

    public Result<User> ModifyEmail(Email email)
    {
        Email = email;

        return this;
    }

    public Result<User> SetTeam(Guid teamId)
    {
        TeamId = teamId;

        return this;
    }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
