using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.GoalAggregate.Queries;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class ToggleActionItemCompletionCommandHandler : ICommandHandler<ToggleActionItemCompletionCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public ToggleActionItemCompletionCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(ToggleActionItemCompletionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var goalWithActionItems = await _databaseContext
                .Set<Goal>()
                .Include(goal => goal.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.GoalId, cancellationToken);

            if (goalWithActionItems is null) return Result.Failure<Unit>(GoalErrors.GoalNotFound(request.GoalId));

            var actionItem = goalWithActionItems.ActionItems.FirstOrDefault(actionItem => actionItem.Id == request.ActionItemId);

            if (actionItem is null) return Result.Failure<Unit>(GoalErrors.ActionItemNotFound(request.ActionItemId));

            actionItem.ToggleCompletion();

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
