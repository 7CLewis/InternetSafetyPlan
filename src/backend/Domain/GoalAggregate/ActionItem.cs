using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Domain.GoalAggregate;

public class ActionItem : Entity
{
    public Guid GoalId { get; private set; }
    public ActionItemName Name { get; private set; }
    public Description? Description { get; private set; }
    public DueDate? DueDate { get; private set; }
    public bool IsComplete { get; private set; } = false;

    private ActionItem(Guid id, Guid goalId, ActionItemName name, Description? description = null, DueDate? dueDate = null)
        : base(id)
    {
        GoalId = goalId;
        Name = name;
        Description = description;
        DueDate = dueDate;
    }

    public static Result<ActionItem> Create(Guid id, Guid goalId, ActionItemName name, Description? description = null, DueDate? dueDate = null)
    {
        var actionItem = new ActionItem(id, goalId, name, description, dueDate);

        return actionItem;
    }

    public Result<ActionItem> Update(ActionItemName name, Description? description = null, DueDate? dueDate = null)
    {
        if (IsComplete) return Result.Failure<ActionItem>(SharedDomainErrors.UpdateAttemptWhenComplete(nameof(ActionItem)));

        Name = name;
        Description = description;
        DueDate = dueDate;

        return this;
    }

    public void ToggleCompletion() => IsComplete = !IsComplete;

    /// <summary>
    /// EF Core Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ActionItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
