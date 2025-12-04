using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Queries;

public class GetUltimateGoalByIdQueryHandler : IQueryHandler<GetUltimateGoalByIdQuery, UltimateGoalByIdResponse>
{
    private readonly IDatabaseContext _databaseContext;
    public GetUltimateGoalByIdQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<UltimateGoalByIdResponse>> Handle(GetUltimateGoalByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ultimateGoal = await _databaseContext
            .Set<UltimateGoal>()
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (ultimateGoal is null) return Result.Failure<UltimateGoalByIdResponse>(UltimateGoalErrors.UltimateGoalNotFound(request.Id));

            var ultimateGoalResponse = ultimateGoal.Adapt<UltimateGoalByIdResponse>();

            return ultimateGoalResponse;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<UltimateGoalByIdResponse>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
