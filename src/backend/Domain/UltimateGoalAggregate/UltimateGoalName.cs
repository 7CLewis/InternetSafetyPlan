using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.UltimateGoalAggregate;

public class UltimateGoalName : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private UltimateGoalName(string value)
    {
        Value = value;
    }

    public static Result<UltimateGoalName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<UltimateGoalName>(SharedDomainErrors.Empty(nameof(UltimateGoalName)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<UltimateGoalName>(SharedDomainErrors.ExceedsMaxLength(nameof(UltimateGoalName), MaxLength));
        }

        var ultimateGoal = new UltimateGoalName(value);

        return ultimateGoal;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
