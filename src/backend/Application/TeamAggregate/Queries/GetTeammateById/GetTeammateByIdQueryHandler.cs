using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.TeamAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Queries;

public class GetTeammateByIdQueryHandler : IQueryHandler<GetTeammateByIdQuery, TeammateByIdResponse>
{
    private readonly IDatabaseContext _databaseContext;
    public GetTeammateByIdQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<TeammateByIdResponse>> Handle(GetTeammateByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var team = await _databaseContext
                .Set<Team>()
                .Include(team => team.Teammates)
                .SingleOrDefaultAsync(e => e.Id == request.TeamId, cancellationToken);
            if (team is null) return Result.Failure<TeammateByIdResponse>(TeamErrors.TeamNotFound(request.TeamId));

            var teammate = team.Teammates.SingleOrDefault(teammate => teammate.Id == request.TeammateId);
            if (teammate is null) return Result.Failure<TeammateByIdResponse>(TeamErrors.TeammateNotFound(request.TeammateId));

            var teammateResponse = teammate.Adapt<TeammateByIdResponse>();

            return teammateResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<TeammateByIdResponse>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
