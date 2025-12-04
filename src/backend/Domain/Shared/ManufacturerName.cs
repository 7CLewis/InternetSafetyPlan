using InternetSafetyPlan.Domain.Base;

namespace InternetSafetyPlan.Domain.Shared;

public class ManufacturerName : ValueObject
{
    public const int MaxLength = 500;

    public string Value { get; init; }

    private ManufacturerName(string value)
    {
        Value = value;
    }

    public static Result<ManufacturerName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<ManufacturerName>(SharedDomainErrors.Empty(nameof(ManufacturerName)));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<ManufacturerName>(SharedDomainErrors.ExceedsMaxLength(nameof(ManufacturerName), MaxLength));
        }

        var manufacturerName = new ManufacturerName(value);

        return manufacturerName;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
