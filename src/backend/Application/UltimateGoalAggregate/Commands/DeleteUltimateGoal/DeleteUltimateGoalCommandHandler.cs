using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;

public class DeleteUltimateGoalCommandHandler : ICommandHandler<DeleteUltimateGoalCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public DeleteUltimateGoalCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(DeleteUltimateGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ultimateGoal =
                await _databaseContext
                .Set<UltimateGoal>()
                .Include(ultimateGoal => ultimateGoal.Goals)
                .ThenInclude(goal => goal.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (ultimateGoal is null) return Result.Failure<Unit>(UltimateGoalErrors.UltimateGoalNotFound(request.Id));

            ultimateGoal.Delete();

            if (ultimateGoal.Goals.Any())
            {
                foreach(var goal in ultimateGoal.Goals)
                {
                    if (goal.ActionItems.Any())
                    {
                        foreach(var actionItem in goal.ActionItems)
                        {
                            actionItem.Delete();
                        }
                        goal.Delete();
                    }
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
