using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public class DeleteTeamCommandHandler : ICommandHandler<DeleteTeamCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public DeleteTeamCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var team =
                await _databaseContext
                .Set<Team>()
                .Include(team => team.Teammates)
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (team is null) return Result.Failure<Unit>(TeamErrors.TeamNotFound(request.Id));

            team.Delete();

            if (team.Teammates.Any())
            {
                foreach(var teammate in team.Teammates)
                {
                    teammate.Delete();
                }
            }

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
