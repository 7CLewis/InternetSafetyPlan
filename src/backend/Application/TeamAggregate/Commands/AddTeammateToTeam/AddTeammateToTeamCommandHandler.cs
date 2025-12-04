using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;


public class AddTeammateToTeamCommandHandler : ICommandHandler<AddTeammateToTeamCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public AddTeammateToTeamCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(AddTeammateToTeamCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var teamWithTeammates = await _databaseContext
                .Set<Team>()
                .Include(e => e.Teammates)
                .SingleOrDefaultAsync(e => e.Id == request.TeamId, cancellationToken);
            if (teamWithTeammates is null) return Result.Failure<Guid>(TeamErrors.TeamNotFound(request.TeamId));

            var teammateId = Guid.NewGuid();

            var teammateResult = CreateTeammate(teammateId, request.TeamId, request.TeammateName, request.TeammateDateOfBirth, request.UserId);
            if (teammateResult.IsFailure) return Result.Failure<Guid>(teammateResult.Error);

            var teammate = teammateResult.Value;

            var result = teamWithTeammates.AddTeammate(teammate);

            /* This is a workaround for an extremely odd bug. EF Core is throwing the error:
             * Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException
             * when this is not included. It's not properly tracking the
             * child entity creation. My guess now is that it's related to 
             * Owned Entities.
             * 
             * TODO: Resolve this bug and remove this EF Core Add() call.
             */
            _databaseContext.Set<Teammate>().Add(teammate);

            if (result.IsFailure) return Result.Failure<Guid>(result.Error);

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return teammateId;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }

    private static Result<Teammate> CreateTeammate(Guid id, Guid teamId, string name, DateTime dateOfBirth, Guid? userId = null)
    {
        var teammateName = TeammateName.Create(name);
        if (teammateName.IsFailure) return Result.Failure<Teammate>(teammateName.Error);

        var teammateDateOfBirth = DateOfBirth.Create(dateOfBirth);
        if (teammateDateOfBirth.IsFailure) return Result.Failure<Teammate>(teammateDateOfBirth.Error);

        var teammateReuslt = Teammate.Create(id, teamId, teammateName.Value, teammateDateOfBirth.Value, userId);
        if (teammateReuslt.IsFailure) return Result.Failure<Teammate>(teammateReuslt.Error);

        return teammateReuslt.Value;
    }
}
