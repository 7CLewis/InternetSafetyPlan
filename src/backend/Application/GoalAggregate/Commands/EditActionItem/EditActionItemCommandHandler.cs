using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class EditActionItemCommandHandler : ICommandHandler<EditActionItemCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public EditActionItemCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(EditActionItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = ActionItemName.Create(request.Name);
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

            var goalWithActionItems = await _databaseContext
                .Set<Goal>()
                .Include(goal => goal.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.GoalId, cancellationToken);

            if (goalWithActionItems is null) return Result.Failure<Unit>(GoalErrors.GoalNotFound(request.GoalId));

            var actionItem = goalWithActionItems.ActionItems.FirstOrDefault(actionItem => actionItem.Id == request.ActionItemId);

            if (actionItem is null) return Result.Failure<Unit>(GoalErrors.ActionItemNotFound(request.ActionItemId));

            var result = actionItem.Update(
                nameResult.Value, 
                descriptionResult?.Value, 
                dueDateResult?.Value
            );

            if (result.IsFailure) return Result.Failure<Unit>(result.Error);

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
