using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.GoalAggregate.Queries;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class EditGoalAndActionItemsCommandHandler : ICommandHandler<EditGoalAndActionItemsCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public EditGoalAndActionItemsCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(EditGoalAndActionItemsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = GoalName.Create(request.Name);
            if (nameResult.IsFailure)            
            {
                // Log error
                return Result.Failure<Unit>(nameResult.Error);
            }

            var descriptionResult = request.Description == null ? null : Description.Create(request.Description);
            if (descriptionResult != null && descriptionResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(descriptionResult.Error);
            }

            Result<DueDate>? dueDateResult = null;
            if (request.DueDate != null)
            {
                dueDateResult = DueDate.Create((DateTime)request.DueDate);
                if (dueDateResult.IsFailure)
                {
                    // Log error
                    return Result.Failure<Unit>(dueDateResult.Error);
                }
            }

            var goal = await _databaseContext
                .Set<Goal>()
                .Include(goal => goal.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (goal is null) return Result.Failure<Unit>(GoalErrors.GoalNotFound(request.Id));
            goal.Update(nameResult.Value, request.Category, descriptionResult?.Value);

            if (request.ActionItems is not null)
            {
                foreach (var currentActionItem in request.ActionItems)
                {
                    var actionItemNameResult = ActionItemName.Create(currentActionItem.Name);
                    if (actionItemNameResult.IsFailure)
                    {
                        // Log error
                        return Result.Failure<Unit>(actionItemNameResult.Error);
                    }

                    var actionItemDescriptionResult = currentActionItem.Description == null ? null : Description.Create(currentActionItem.Description);
                    if (actionItemDescriptionResult != null && actionItemDescriptionResult.IsFailure)
                    {
                        // Log error
                        return Result.Failure<Unit>(actionItemDescriptionResult.Error);
                    }

                    Result<DueDate>? actionItemDueDateResult = null;
                    if (currentActionItem.DueDate != null)
                    {
                        actionItemDueDateResult = DueDate.Create((DateTime)currentActionItem.DueDate);
                        if (actionItemDueDateResult.IsFailure)
                        {
                            // Log error
                            return Result.Failure<Unit>(actionItemDueDateResult.Error);
                        }
                    }

                    var actionItemId = Guid.NewGuid();

                    var actionItemResult = ActionItem.Create(actionItemId, request.Id, actionItemNameResult.Value, actionItemDescriptionResult?.Value, actionItemDueDateResult?.Value);
                    if (actionItemResult.IsFailure)
                    {
                        // Log error
                        return Result.Failure<Unit>(actionItemResult.Error);
                    }

                    var actionItem = goal.ActionItems.SingleOrDefault(actionItem => actionItem.Id == currentActionItem.Id);
                    if (actionItem is null) return Result.Failure<Unit>(GoalErrors.ActionItemNotFound(currentActionItem.Id));
                    actionItem.Update(actionItemNameResult.Value, actionItemDescriptionResult?.Value, actionItemDueDateResult?.Value);
                }
            }
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Unit>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
