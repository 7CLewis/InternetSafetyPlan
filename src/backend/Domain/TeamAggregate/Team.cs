using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using MediatR;

namespace InternetSafetyPlan.Domain.TeamAggregate;

public class Team : AggregateRoot
{
    public const int TeammateCapacity = 20;
    public const int UserCapacity = 2;
    public const int UltimateGoalCapacity = 3;

    public TeamName Name { get; private set; }
    public Description? Description { get; private set; }

    private readonly List<Teammate> _teammates = [];
    public IEnumerable<Teammate> Teammates => _teammates.AsReadOnly();
    private readonly List<UltimateGoal> _ultimateGoals = [];
    public IEnumerable<UltimateGoal> UltimateGoals => _ultimateGoals.AsReadOnly();

    private readonly List<User> _users = [];
    public IEnumerable<User> Users => _users.AsReadOnly();

    private Team(Guid id, User user, TeamName name, Description? description = null)
        : base(id)
    {
        Name = name;
        Description = description;

        _users.Add(user);

        _teammates.Capacity = TeammateCapacity;
        _ultimateGoals.Capacity = UltimateGoalCapacity;
    }

    public static Result<Team> Create(Guid id, User user, TeamName name, Description? description = null)
    {
        var team = new Team(id, user, name, description);

        return team;
    }

    public Result<Team> UpdateInformation(TeamName name, Description? description = null)
    {
        Name = name;
        Description = description;

        return this;
    }

    public Result<Unit> AddTeammate(Teammate teammate)
    {
        if (_teammates.Contains(teammate))
            return Result.Failure<Unit>(SharedDomainErrors.AlreadyAdded(nameof(Team), nameof(Teammate), teammate.Id));

        if (_teammates.Count >= TeammateCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Team), nameof(Teammate), TeammateCapacity));

        _teammates.Add(teammate);

        return Unit.Value;
    }

    public Result<Unit> DeleteTeammate(Teammate teammate)
    {
        if (!_teammates.Contains(teammate))
            return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(Teammate), teammate.Id));

        teammate.Delete();

        return Unit.Value;
    }

    public Result<Unit> AddUser(User user)
    {
        if (_users.Contains(user))
            return Result.Failure<Unit>(SharedDomainErrors.AlreadyAdded(nameof(Team), nameof(User), user.Id));

        if (_users.Count >= UserCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Team), nameof(User), UserCapacity));

        _users.Add(user);

        return Unit.Value;
    }

    public Result<Unit> RemoveUser(User user)
    {
        if (!_users.Contains(user))
            return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(User), user.Id));

        _users.Remove(user);

        return Unit.Value;
    }

    public Result<Unit> AddUltimateGoal(UltimateGoal ultimateGoal)
    {
        if (_ultimateGoals.Contains(ultimateGoal))
            return Result.Failure<Unit>(SharedDomainErrors.AlreadyAdded(nameof(Team), nameof(UltimateGoal), ultimateGoal.Id));

        if (_ultimateGoals.Count >= UltimateGoalCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Team), nameof(UltimateGoal), UltimateGoalCapacity));
        _ultimateGoals.Add(ultimateGoal);

        return Unit.Value;
    }

    public Result<Unit> RemoveUltimateGoal(UltimateGoal ultimateGoal)
    {
        if (!_ultimateGoals.Contains(ultimateGoal))
            return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(UltimateGoal), ultimateGoal.Id));
        _ultimateGoals.Remove(ultimateGoal);

        return Unit.Value;
    }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Team() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
