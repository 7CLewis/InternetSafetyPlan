using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class CreateGoalAndActionItemsCommandHandler : ICommandHandler<CreateGoalAndActionItemsCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public CreateGoalAndActionItemsCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(CreateGoalAndActionItemsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = GoalName.Create(request.Name);
            if (nameResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(nameResult.Error);
            }

            var descriptionResult = request.Description == null ? null : Description.Create(request.Description);
            if (descriptionResult != null && descriptionResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(descriptionResult.Error);
            }

            Result<DueDate>? dueDateResult = null;
            if (request.DueDate != null)
            {
                dueDateResult = DueDate.Create((DateTime)request.DueDate);
                if (dueDateResult.IsFailure)
                {
                    // Log error
                    return Result.Failure<Guid>(dueDateResult.Error);
                }
            }

            var goalId = Guid.NewGuid();

            var goalResult = Goal.Create(goalId, request.UltimateGoalId, nameResult.Value, request.Category, descriptionResult?.Value, dueDateResult?.Value);
            if (goalResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(goalResult.Error);
            }

            var goal = goalResult.Value;

            await _databaseContext.Set<Goal>().AddAsync(goal, cancellationToken);

            if (request.ActionItems is not null)
            {
                foreach (var currentActionItem in request.ActionItems)
                {
                    var actionItemNameResult = ActionItemName.Create(currentActionItem.Name);
                    if (actionItemNameResult.IsFailure)
                    {
                        // Log error
                        return Result.Failure<Guid>(actionItemNameResult.Error);
                    }

                    var actionItemDescriptionResult = currentActionItem.Description == null ? null : Description.Create(currentActionItem.Description);
                    if (actionItemDescriptionResult != null && actionItemDescriptionResult.IsFailure)
                    {
                        // Log error
                        return Result.Failure<Guid>(actionItemDescriptionResult.Error);
                    }

                    Result<DueDate>? actionItemDueDateResult = null;
                    if (currentActionItem.DueDate != null)
                    {
                        actionItemDueDateResult = DueDate.Create((DateTime)currentActionItem.DueDate);
                        if (actionItemDueDateResult.IsFailure)
                        {
                            // Log error
                            return Result.Failure<Guid>(actionItemDueDateResult.Error);
                        }
                    }

                    var actionItemId = Guid.NewGuid();

                    var actionItemResult = ActionItem.Create(actionItemId, goalId, actionItemNameResult.Value, actionItemDescriptionResult?.Value, actionItemDueDateResult?.Value);
                    if (actionItemResult.IsFailure)
                    {
                        // Log error
                        return Result.Failure<Guid>(actionItemResult.Error);
                    }

                    var actionItem = actionItemResult.Value;
                    goal.AddActionItem(actionItem);
                    _databaseContext.Set<ActionItem>().Add(actionItem);
                }
            }
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return goalId;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
