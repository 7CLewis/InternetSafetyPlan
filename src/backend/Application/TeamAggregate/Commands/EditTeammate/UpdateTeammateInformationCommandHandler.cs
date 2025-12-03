using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;


public class UpdateTeammateInformationCommandHandler : ICommandHandler<UpdateTeammateInformationCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public UpdateTeammateInformationCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(UpdateTeammateInformationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = TeamName.Create(request.Name);
            if (nameResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(nameResult.Error);
            }

            var dateOfBirthResult = DateOfBirth.Create(request.DateOfBirth);
            if (dateOfBirthResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(dateOfBirthResult.Error);
            }

            var teamWithTeammates = await _databaseContext
                .Set<Team>()
                .Include(team => team.Teammates)
                .SingleOrDefaultAsync(e => e.Id == request.TeamId, cancellationToken);
            if (teamWithTeammates is null) return Result.Failure<Unit>(TeamErrors.TeamNotFound(request.TeamId));

            var teammate = teamWithTeammates.Teammates.FirstOrDefault(teammate => teammate.Id == request.TeammateId);

            if (teammate is null) return Result.Failure<Unit>(TeamErrors.TeammateNotFound(request.TeammateId));

            var result = teammate.UpdateInformation(
                TeammateName.Create(request.Name).Value,
                DateOfBirth.Create(request.DateOfBirth).Value,
                request.UserId);

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
