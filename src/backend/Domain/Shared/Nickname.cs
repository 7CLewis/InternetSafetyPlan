using InternetSafetyPlan.Domain.Base;

namespace InternetSafetyPlan.Domain.Shared;

public class Nickname : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private Nickname(string value)
    {
        Value = value;
    }

    public static Result<Nickname> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Nickname>(SharedDomainErrors.Empty(nameof(Nickname)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<Nickname>(SharedDomainErrors.ExceedsMaxLength(nameof(Nickname), MaxLength));
        }

        var nickname = new Nickname(value);

        return nickname;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
