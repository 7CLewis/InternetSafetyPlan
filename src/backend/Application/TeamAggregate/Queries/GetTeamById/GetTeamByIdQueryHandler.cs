using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.TeamAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Queries;

public class GetTeamByIdQueryHandler : IQueryHandler<GetTeamByIdQuery, TeamByIdResponse>
{
    private readonly IDatabaseContext _databaseContext;
    public GetTeamByIdQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<TeamByIdResponse>> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var team = await _databaseContext
                .Set<Team>()
                .Include(team => team.Teammates)
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (team is null) return Result.Failure<TeamByIdResponse>(TeamErrors.TeamNotFound(request.Id));

            var teamResponse = team.Adapt<TeamByIdResponse>();

            return teamResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<TeamByIdResponse>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
