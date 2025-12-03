using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;

namespace InternetSafetyPlan.Domain.DeviceAggregate;

public class Device : AggregateRoot
{
    public const int TagCapacity = 50;
    public const int TeammateCapacity = Team.TeammateCapacity;

    public Guid TeamId { get; private set; }
    public ManufacturerName Name { get; private set; }
    public Nickname Nickname { get; private set; }
    public DeviceType Type { get; private set; }

    private readonly List<Tag> _tags = [];
    public IEnumerable<Tag> Tags => _tags.AsReadOnly();

    private readonly List<Teammate> _teammates = [];
    public IEnumerable<Teammate> Teammates => _teammates.AsReadOnly();

    private Device(Guid id, Guid teamId, ManufacturerName name, Nickname nickname, DeviceType type)
        : base(id)
    {
        TeamId = teamId;
        Name = name;
        Nickname = nickname;
        Type = type;

        _tags.Capacity = TagCapacity;
        _teammates.Capacity = TeammateCapacity;
    }

    public static Result<Device> Create(Guid id, Guid teamId, ManufacturerName name, Nickname nickname, DeviceType type)
    {
        var device = new Device(id, teamId, name, nickname, type);

        return device;
    }

    public Result<Device> UpdateInformation(ManufacturerName name, Nickname nickname, DeviceType type)
    {
        Name = name;
        Nickname = nickname;
        Type = type;

        return this;
    }

    public Result<Unit> AddTag(Tag tag)
    {
        if (_tags.Contains(tag))
            return Result.Failure<Unit>(SharedDomainErrors.TagAlreadyAdded(tag.Name, tag.Type, nameof(Device)));

        if (_tags.Count >= TagCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Device), nameof(Tag), TagCapacity));

        _tags.Add(tag);

        return Unit.Value;
    }

    public Result<Unit> RemoveTag(Tag tag)
    {
        if (!_tags.Contains(tag))
            return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(Tag), tag.Id));

        _tags.Remove(tag);

        return Unit.Value;
    }

    public Result<Unit> AddTeammate(Teammate teammate)
    {
        if (_teammates.Contains(teammate))
            return Result.Failure<Unit>(SharedDomainErrors.AlreadyAdded(nameof(Device), nameof(Teammate), teammate.Id));

        if (_teammates.Count >= TeammateCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Device), nameof(Teammate), TeammateCapacity));

        _teammates.Add(teammate);

        return Unit.Value;
    }

    public Result<Unit> RemoveTeammate(Teammate teammate)
    {
        if (!_teammates.Contains(teammate)) return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(Teammate), teammate.Id));

        _teammates.Remove(teammate);

        return Unit.Value;
    }

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Device() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

public enum DeviceType
{
    Phone = 1,
    TV = 2,
    Router = 3,
    Watch = 4,
    Tablet = 5,
    HomeAssistant = 6,
    PC = 7,
    Chromecast = 8,
    EReader = 9,
    Vehicle = 10
}
