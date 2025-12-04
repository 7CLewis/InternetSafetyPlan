using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;


public class UpdateTeamInformationCommandHandler : ICommandHandler<UpdateTeamInformationCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public UpdateTeamInformationCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(UpdateTeamInformationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = TeamName.Create(request.Name);
            if (nameResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(nameResult.Error);
            }

            var descriptionResult = Description.Create(request.Description);
            if (descriptionResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(descriptionResult.Error);
            }

            var team = await _databaseContext
                .Set<Team>()
                .SingleOrDefaultAsync(e => e.Id == request.TeamId, cancellationToken);
            if (team is null) return Result.Failure<Unit>(TeamErrors.TeamNotFound(request.TeamId));

            var result = team.UpdateInformation(nameResult.Value, descriptionResult.Value);

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
