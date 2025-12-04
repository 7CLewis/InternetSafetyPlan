using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using InternetSafetyPlan.Shared;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public class GetSuggestedGoalsQueryHandler : IQueryHandler<GetSuggestedGoalsQuery, List<SuggestedGoalsResponse>>
{
    private readonly IDatabaseContext _databaseContext;
    public GetSuggestedGoalsQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<SuggestedGoalsResponse>>> Handle(GetSuggestedGoalsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Goal> goals = [];
            var goalLists = await _databaseContext
                .Set<UltimateGoal>()
                .Include(e => e.Goals)
                .ThenInclude(goal => goal.ActionItems)
                .Where(e => e.TeamId == SharedVariables.SuggestedGoalsTeamId)
                .Select(e => e.Goals.ToList()).ToListAsync(cancellationToken);
            goalLists.ForEach(l => goals.AddRange(l));

            if (goals is null || goals.Count == 0) return Result.Success(new List<SuggestedGoalsResponse>());

            var teamGoalsResponse = goals.Adapt<List<SuggestedGoalsResponse>>();

            return teamGoalsResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<List<SuggestedGoalsResponse>>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
