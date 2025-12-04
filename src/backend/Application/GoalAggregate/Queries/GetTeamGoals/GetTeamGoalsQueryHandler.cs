using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public class GetTeamGoalsQueryHandler : IQueryHandler<GetTeamGoalsQuery, List<TeamGoalsResponse>>
{
    private readonly IDatabaseContext _databaseContext;
    public GetTeamGoalsQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<TeamGoalsResponse>>> Handle(GetTeamGoalsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Goal> goals = [];
            var goalLists = await _databaseContext
                .Set<UltimateGoal>()
                .Include(e => e.Goals)
                .ThenInclude(goal => goal.ActionItems)
                .Where(e => e.TeamId == request.TeamId)
                .Select(e => e.Goals.ToList()).ToListAsync(cancellationToken);
            goalLists.ForEach(l => goals.AddRange(l));

            if (goals is null || goals.Count == 0) return Result.Success(new List<TeamGoalsResponse>());

            var teamGoalsResponse = goals.Adapt<List<TeamGoalsResponse>>();

            return teamGoalsResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<List<TeamGoalsResponse>>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
