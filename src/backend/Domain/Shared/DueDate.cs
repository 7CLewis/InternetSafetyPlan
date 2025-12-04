using InternetSafetyPlan.Domain.Base;

namespace InternetSafetyPlan.Domain.Shared;

public class DueDate : ValueObject
{
    public DateTime Value { get; init; }

    private DueDate(DateTime value)
    {
        Value = value;
    }

    public static Result<DueDate> Create(DateTime value)
    {
        if (value.Date < DateTimeOffset.UtcNow.Date)
            return Result.Failure<DueDate>(DueDateErrors.DueDateInPast());

        var dueDate = new DueDate(value);

        return dueDate;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public static class DueDateErrors
{
    public static Error DueDateInPast() => new("DueDate.Passed", "DueDate has already passed.");
}
