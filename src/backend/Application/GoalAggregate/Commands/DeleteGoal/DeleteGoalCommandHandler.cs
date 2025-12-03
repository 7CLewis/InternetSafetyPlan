using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public class DeleteGoalCommandHandler : ICommandHandler<DeleteGoalCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public DeleteGoalCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(DeleteGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var goal =
                await _databaseContext
                .Set<Goal>()
                .Include(goal => goal.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (goal is null) return Result.Failure<Unit>(GoalErrors.GoalNotFound(request.Id));

            goal.Delete();

            if (goal.ActionItems.Any())
            {
                foreach (var actionItem in goal.ActionItems)
                {
                    actionItem.Delete();
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
