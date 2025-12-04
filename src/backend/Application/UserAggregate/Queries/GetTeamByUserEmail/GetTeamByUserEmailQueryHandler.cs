using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public class GetTeamByUserEmailQueryHandler : IQueryHandler<GetTeamByUserEmailQuery, TeamByUserEmailResponse?>
{
    private readonly IDatabaseContext _databaseContext;
    public GetTeamByUserEmailQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<TeamByUserEmailResponse?>> Handle(GetTeamByUserEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _databaseContext
                .Set<User>()
                .SingleOrDefaultAsync(e => e.Email.Value == request.Email, cancellationToken);
            if (user is null) return Result.Success<TeamByUserEmailResponse?>(null);

            var team = await _databaseContext
                .Set<Team>()
                .SingleOrDefaultAsync(e => e.Id == user.TeamId, cancellationToken);
            if (team is null) return Result.Success<TeamByUserEmailResponse?>(null);

            var teamResponse = team.Adapt<TeamByUserEmailResponse?>();

            return teamResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<TeamByUserEmailResponse?>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
