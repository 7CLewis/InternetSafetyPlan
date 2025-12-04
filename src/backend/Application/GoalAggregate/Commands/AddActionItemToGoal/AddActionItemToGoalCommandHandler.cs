using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

// TODO: Rename to differntiate as a child entity Create? Versus a regular
// "add" of an existing entity. Maybe CreateActionItemForGoalCommand(Handler)?
public class AddActionItemToGoalCommandHandler : ICommandHandler<AddActionItemToGoalCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public AddActionItemToGoalCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(AddActionItemToGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var goalWithActionItems = await _databaseContext
                .Set<Goal>()
                .Include(e => e.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.GoalId, cancellationToken);
            if (goalWithActionItems is null) return Result.Failure<Guid>(GoalErrors.GoalNotFound(request.GoalId));

            var actionItemId = Guid.NewGuid();

            var actionItemResult = CreateActionItem(actionItemId, request.GoalId, request.ActionItemName, request.ActionItemDescription, request.ActionItemDueDate);
            if (actionItemResult.IsFailure) return Result.Failure<Guid>(actionItemResult.Error);

            var actionItem = actionItemResult.Value;

            var result = goalWithActionItems.AddActionItem(actionItem);

            if (result.IsFailure) return Result.Failure<Guid>(result.Error);

            /* This is a workaround for an extremely odd bug. EF Core is throwing the error:
             * Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException
             * when this is not included. It's not properly tracking the
             * child entity creation. My guess now is that it's related to 
             * Owned Entities and that I just don't fully understand how 
             * tracking works.
             * 
             * TODO: Resolve this bug and remove this EF Core Add() call.
             */
            _databaseContext.Set<ActionItem>().Add(actionItem);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return actionItemId;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }

    private static Result<ActionItem> CreateActionItem(Guid id, Guid goalId, string name, string? description = null, DateTime? dueDate = null)
    {
        var nameResult = ActionItemName.Create(name);
        if (nameResult.IsFailure) return Result.Failure<ActionItem>(nameResult.Error);

        Result<Description>? descriptionResult = null;
        if (description != null)
        {
            descriptionResult = Description.Create(description);
            if (descriptionResult.IsFailure) return Result.Failure<ActionItem>(descriptionResult.Error);
        }

        Result<DueDate>? dueDateResult = null;
        if (dueDate != null)
        {
            dueDateResult = DueDate.Create((DateTime)dueDate);
            if (dueDateResult.IsFailure) return Result.Failure<ActionItem>(dueDateResult.Error);
        }

        var actionItemResult = ActionItem.Create(id, goalId, nameResult.Value, descriptionResult?.Value, dueDateResult?.Value);
        if (actionItemResult.IsFailure) return Result.Failure<ActionItem>(actionItemResult.Error);

        return actionItemResult.Value;
    }
}
