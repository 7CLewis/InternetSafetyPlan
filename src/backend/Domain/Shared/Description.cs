using InternetSafetyPlan.Domain.Base;

namespace InternetSafetyPlan.Domain.Shared;

public class Description : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        if (value.Length > MaxLength)
        {
            return Result.Failure<Description>(SharedDomainErrors.ExceedsMaxLength(nameof(Description), MaxLength));
        }

        var description = new Description(value);

        return description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
