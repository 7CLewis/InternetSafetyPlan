using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class DeleteActionItemCommandHandler : ICommandHandler<DeleteActionItemCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public DeleteActionItemCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(DeleteActionItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var actionItem = await
                _databaseContext
                .Set<ActionItem>()
                .SingleOrDefaultAsync(e => e.Id == request.ActionItemId, cancellationToken);
            if (actionItem is null) return Result.Failure<Unit>(GoalErrors.ActionItemNotFound(request.ActionItemId));

            var goalWithActionItems = await _databaseContext
                .Set<Goal>()
                .Include(e => e.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.GoalId, cancellationToken);
            if (goalWithActionItems is null) return Result.Failure<Unit>(GoalErrors.GoalNotFound(request.GoalId));

            var result = goalWithActionItems.DeleteActionItem(actionItem);

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
