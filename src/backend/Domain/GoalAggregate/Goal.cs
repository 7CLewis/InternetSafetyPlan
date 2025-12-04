using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;

namespace InternetSafetyPlan.Domain.GoalAggregate;

public class Goal : AggregateRoot
{
    public const int ActionItemCapacity = 20;
    public const int DeviceCapacity = 30;
    public const int TeammateCapacity = Team.TeammateCapacity;
    public const int TagCapacity = 50;

    public Guid UltimateGoalId { get; private set; }
    public GoalName Name { get; private set; }
    public Description? Description { get; private set; }
    public GoalCategory Category { get; private set; }
    public DueDate? DueDate { get; private set; }
    public bool IsComplete { get; private set; } = false;
    private readonly List<ActionItem> _actionItems = [];
    public IEnumerable<ActionItem> ActionItems => _actionItems.AsReadOnly();

    private readonly List<Device> _affectedDevices = [];
    public IEnumerable<Device> AffectedDevices => _affectedDevices.AsReadOnly();

    private readonly List<Teammate> _affectedTeammates = [];
    public IEnumerable<Teammate> AffectedTeammates => _affectedTeammates.AsReadOnly();

    private readonly List<Tag> _tags = [];
    public IEnumerable<Tag> Tags => _tags.AsReadOnly();

    private Goal(Guid id, Guid ultimateGoalId, GoalName name, GoalCategory category, Description? description = null, DueDate? dueDate = null)
        : base(id)
    {
        UltimateGoalId = ultimateGoalId;
        Name = name;
        Category = category;
        Description = description;
        DueDate = dueDate;
    }

    public static Result<Goal> Create(Guid id, Guid ultimateGoalId, GoalName name, GoalCategory category, Description? description = null, DueDate? dueDate = null)
    {
        var goal = new Goal(id, ultimateGoalId, name, category, description, dueDate);

        return goal;
    }

    public Result<Goal> Update(GoalName name, GoalCategory category, Description? description = null)
    {
        if (IsComplete)
            return Result.Failure<Goal>(SharedDomainErrors.UpdateAttemptWhenComplete(nameof(Goal)));

        Name = name;
        Category = category;
        Description = description;

        return this;
    }

    public void ToggleCompletion() => IsComplete = !IsComplete;

    public Result<Unit> AddActionItem(ActionItem actionItem)
    {
        if (_actionItems.Contains(actionItem))
            return Result.Failure<Unit>(SharedDomainErrors.AlreadyAdded(nameof(Goal), nameof(ActionItem), actionItem.Id));

        if (_actionItems.Count >= ActionItemCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Goal), nameof(ActionItem), ActionItemCapacity));

        _actionItems.Add(actionItem);

        return Unit.Value;
    }

    public Result<Unit> DeleteActionItem(ActionItem actionItem)
    {
        if (!_actionItems.Contains(actionItem))
            return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(ActionItem), actionItem.Id));

        actionItem.Delete();

        return Unit.Value;
    }

    public Result<Unit> AddAffectedDevice(Device device)
    {
        if (_affectedDevices.Contains(device))
            return Result.Failure<Unit>(SharedDomainErrors.AlreadyAdded(nameof(Goal), nameof(Device), device.Id));

        if (_affectedDevices.Count >= DeviceCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Goal), nameof(Device), DeviceCapacity));

        _affectedDevices.Add(device);

        return Unit.Value;
    }

    public Result<Unit> RemoveAffectedDevice(Device device)
    {
        if (!_affectedDevices.Contains(device))
            return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(Device), device.Id));

        _affectedDevices.Remove(device);

        return Unit.Value;
    }

    public Result<Unit> AddAffectedTeammate(Teammate teammate)
    {
        if (_affectedTeammates.Contains(teammate))
            return Result.Failure<Unit>(SharedDomainErrors.AlreadyAdded(nameof(Goal), nameof(Teammate), teammate.Id));

        if (_affectedTeammates.Count >= Team.TeammateCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Goal), nameof(Teammate), TeammateCapacity));

        _affectedTeammates.Add(teammate);

        return Unit.Value;
    }

    public Result<Unit> RemoveAffectedTeammate(Teammate teammate)
    {
        if (!_affectedTeammates.Contains(teammate))
            return Result.Failure<Unit>(SharedDomainErrors.NotFoundInList(nameof(Teammate), teammate.Id));

        _affectedTeammates.Remove(teammate);

        return Unit.Value;
    }

    public Result<Unit> AddTag(Tag tag)
    {
        if (_tags.Contains(tag))
            return Result.Failure<Unit>(SharedDomainErrors.TagAlreadyAdded(tag.Name, tag.Type, nameof(Goal)));

        if (_tags.Count >= TagCapacity)
            return Result.Failure<Unit>(SharedDomainErrors.MaxCapacity(nameof(Goal), nameof(Tag), TagCapacity));

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

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Goal() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}

public enum GoalCategory
{
    Communication = 1,
    SocialMedia = 2,
    Phones = 3,
    Toddlers = 4,
    Children = 5,
    Preteens = 6,
    Teens = 7,
    Discipline = 8,
    VideoGames = 9,
    Cybersecurity = 10,
    ExplicitContent = 11,
    FamilyCulture = 12,
    Filtering = 13,
    Uncategorized = 14,
    WiFi = 15,
    Education = 16
}
