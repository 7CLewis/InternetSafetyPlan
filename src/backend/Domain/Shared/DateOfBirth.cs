using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Shared;

namespace InternetSafetyPlan.Domain.Shared;

public class DateOfBirth : ValueObject
{
    public DateTime Value { get; init; }

    private DateOfBirth(DateTime value)
    {
        Value = value;
    }

    public static Result<DateOfBirth> Create(DateTime value)
    {
        var validDateOfBirthRange = new DateTimeRange(
            new DateTime(1900, 1, 1),
            DateTime.UtcNow
        );
        if (!validDateOfBirthRange.Includes(value))
            return Result.Failure<DateOfBirth>(DateOfBirthErrors.OutOfRange());

        var dateofBirth = new DateOfBirth(value);

        return dateofBirth;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public static class DateOfBirthErrors
{
    public static Error OutOfRange() => new("DateOfBirth.OutOfRange", "DateOfBirth not within acceptable range.");
}
