using InternetSafetyPlan.Domain.Base;

namespace InternetSafetyPlan.Domain.Shared;

public class Tag : ValueObject
{
    public const int NameMaxLength = 50;
    public const int TypeMaxLength = 20;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public TagType Type { get; private set; }

    private Tag(Guid id, string name, TagType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public static Result<Tag> Create(Guid id, string name, TagType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            // TODO: Generic class with failures;
            // call shared validator class and return
            // a result object with all errors (or maybe just one idk)
            return Result.Failure<Tag>(SharedDomainErrors.Empty(nameof(Name)));
        }

        if (name.Length > NameMaxLength)
        {
            return Result.Failure<Tag>(SharedDomainErrors.ExceedsMaxLength(nameof(Name), NameMaxLength));
        }

        var tag = new Tag(id, name, type);

        return tag;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Type;
    }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Tag() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

public enum TagType
{
    DeviceMetadata = 1,
    Application = 2,
    RealWorld = 3,
    Other = 4
}
