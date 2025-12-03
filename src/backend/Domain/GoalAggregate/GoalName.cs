using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.GoalAggregate;

public class GoalName : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private GoalName(string value)
    {
        Value = value;
    }

    public static Result<GoalName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<GoalName>(SharedDomainErrors.Empty(nameof(GoalName)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<GoalName>(SharedDomainErrors.ExceedsMaxLength(nameof(GoalName), MaxLength));
        }

        var goalName = new GoalName(value);

        return goalName;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
