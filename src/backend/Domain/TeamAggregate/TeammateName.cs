using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.TeamAggregate;

public class TeammateName : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private TeammateName(string value)
    {
        Value = value;
    }

    public static Result<TeammateName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<TeammateName>(SharedDomainErrors.Empty(nameof(TeammateName)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<TeammateName>(SharedDomainErrors.ExceedsMaxLength(nameof(TeammateName), MaxLength));
        }

        var teammate = new TeammateName(value);

        return teammate;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
