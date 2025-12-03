namespace InternetSafetyPlan.Domain.Shared;

public class Schedule
{
    public DateTime Start { get; private set; }
    public DateTime? End { get; private set; }
    public Day[]? RecurringDays { get; private set; }
    public byte RecurringFrequency { get; private set; }
    public RecurrenceType RecurrenceType { get; private set; }

    public Schedule(
        DateTime start,
        DateTime? end,
        Day[]? recurringDays,
        byte recurringFrequency,
        RecurrenceType recurrenceType
    )
    {
        Start = start >= end ? throw new Exception("Start must be before end") : start;
        End = end <= start ? throw new Exception("End must be later in time than Start") : end;
        RecurringDays = recurringDays?.Length > 7 ? throw new Exception("Can only have a maximum of 7 RecurringDays") : recurringDays;
        RecurringFrequency = recurringFrequency;
        RecurrenceType = recurrenceType;
    }
}

public enum Day
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}

public enum RecurrenceType
{
    Day = 1,
    Week = 2,
    Month = 3,
    Year = 4
}
