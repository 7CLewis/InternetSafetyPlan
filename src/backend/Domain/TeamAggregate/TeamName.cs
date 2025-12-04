using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.TeamAggregate;

public class TeamName : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private TeamName(string value)
    {
        Value = value;
    }

    public static Result<TeamName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<TeamName>(SharedDomainErrors.Empty(nameof(TeamName)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<TeamName>(SharedDomainErrors.ExceedsMaxLength(nameof(TeamName), MaxLength));
        }

        var team = new TeamName(value);

        return team;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
