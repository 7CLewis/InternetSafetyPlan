using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.GoalAggregate;

public class ActionItemName : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private ActionItemName(string value)
    {
        Value = value;
    }

    public static Result<ActionItemName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<ActionItemName>(SharedDomainErrors.Empty(nameof(ActionItemName)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<ActionItemName>(SharedDomainErrors.ExceedsMaxLength(nameof(ActionItemName), MaxLength));
        }

        var actionItemName = new ActionItemName(value);

        return actionItemName;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
