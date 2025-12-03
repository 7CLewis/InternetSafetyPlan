using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public class GetGoalByIdQueryHandler : IQueryHandler<GetGoalByIdQuery, GoalByIdResponse>
{
    private readonly IDatabaseContext _databaseContext;
    public GetGoalByIdQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GoalByIdResponse>> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var goal = await _databaseContext
                .Set<Goal>()
                .Include(goal => goal.ActionItems)
                .SingleOrDefaultAsync(e => e.Id == request.GoalId, cancellationToken);
            if (goal is null) return Result.Failure<GoalByIdResponse>(GoalErrors.GoalNotFound(request.GoalId));

            var goalByIdResponse = goal.Adapt<GoalByIdResponse>();

            return Result.Success(goalByIdResponse);
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<GoalByIdResponse>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
