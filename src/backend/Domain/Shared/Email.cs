using InternetSafetyPlan.Domain.Base;

namespace InternetSafetyPlan.Domain.Shared;

public class Email : ValueObject
{
    public const int MaxLength = 255;
    public string Value { get; init; }

    private Email(string value)
    {
        Value = value;
    }

    // TODO: Add email regex validation
    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Email>(SharedDomainErrors.Empty(nameof(Email)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<Email>(SharedDomainErrors.ExceedsMaxLength(nameof(Email), MaxLength));
        }

        var email = new Email(value);

        return email;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
