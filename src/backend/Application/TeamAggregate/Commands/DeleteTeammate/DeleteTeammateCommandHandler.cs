using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;


public class DeleteTeammateCommandHandler : ICommandHandler<DeleteTeammateCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public DeleteTeammateCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(DeleteTeammateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var teammate = await
                _databaseContext
                .Set<Teammate>()
                .SingleOrDefaultAsync(e => e.Id == request.TeammateId, cancellationToken);
            if (teammate is null) return Result.Failure<Unit>(TeamErrors.TeammateNotFound(request.TeammateId));

            var teamWithTeammates = await _databaseContext
                .Set<Team>()
                .Include(e => e.Teammates)
                .SingleOrDefaultAsync(e => e.Id == request.TeamId, cancellationToken);
            if (teamWithTeammates is null) return Result.Failure<Unit>(TeamErrors.TeamNotFound(request.TeamId));

            var result = teamWithTeammates.DeleteTeammate(teammate);

            if (result.IsFailure) return Result.Failure<Unit>(result.Error);

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Unit>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
