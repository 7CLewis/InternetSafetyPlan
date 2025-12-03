using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public class GetTeamActionItemsQueryHandler : IQueryHandler<GetTeamActionItemsQuery, List<TeamActionItemsResponse>>
{
    private readonly IDatabaseContext _databaseContext;
    public GetTeamActionItemsQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<TeamActionItemsResponse>>> Handle(GetTeamActionItemsQuery request, CancellationToken cancellationToken)
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

            if (goals is null || goals.Count == 0) return Result.Success(new List<TeamActionItemsResponse>());

            List<TeamActionItemsResponse> teamActionItemsResponse = new List<TeamActionItemsResponse>();
            goals.ForEach(goal =>
            {
                goal.ActionItems.ToList().ForEach(actionItem =>
                {
                    teamActionItemsResponse.Add(new TeamActionItemsResponse(actionItem.Id, goal.Id, goal.Name.Value, actionItem.Name.Value, actionItem.DueDate?.Value ?? DateTime.MinValue, actionItem.IsComplete, actionItem.Description?.Value));
                });
            });

            return teamActionItemsResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<List<TeamActionItemsResponse>>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
