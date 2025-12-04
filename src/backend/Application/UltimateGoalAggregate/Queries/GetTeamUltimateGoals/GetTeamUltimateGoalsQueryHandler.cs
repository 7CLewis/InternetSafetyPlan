using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Queries;

public class GetTeamUltimateGoalsQueryHandler : IQueryHandler<GetTeamUltimateGoalsQuery, List<TeamUltimateGoalsResponse>>
{
    private readonly IDatabaseContext _databaseContext;
    public GetTeamUltimateGoalsQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<TeamUltimateGoalsResponse>>> Handle(GetTeamUltimateGoalsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ultimateGoals = request.IncludeGoalsAndActions ?
                await _databaseContext
                    .Set<UltimateGoal>()
                    .Include(e => e.Goals)
                    .ThenInclude(e => e.ActionItems)
                    .Where(e => e.TeamId == request.TeamId)
                    .ToListAsync(cancellationToken)
                :
                await _databaseContext
                    .Set<UltimateGoal>()
                    .Where(e => e.TeamId == request.TeamId)
                    .ToListAsync(cancellationToken);

            if (ultimateGoals is null) return Result.Success(new List<TeamUltimateGoalsResponse>());

            var teamUltimateGoalsResponse = ultimateGoals.Adapt<List<TeamUltimateGoalsResponse>>();

            return teamUltimateGoalsResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<List<TeamUltimateGoalsResponse>>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
