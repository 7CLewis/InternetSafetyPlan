using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.UserAggregate;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.TeamAggregate.Commands;

public class CreateTeamCommandHandler : ICommandHandler<CreateTeamCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public CreateTeamCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = TeamName.Create(request.Name);
            if (nameResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(nameResult.Error);
            }

            var descriptionResult = request.Description == null ? null : Description.Create(request.Description);
            if (descriptionResult != null && descriptionResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(descriptionResult.Error);
            }

            var emailResult = Email.Create(request.UserEmail);
            if (emailResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(emailResult.Error);
            }

            var user = await _databaseContext
                .Set<User>()
                .SingleOrDefaultAsync(e => e.Email.Value == emailResult.Value.Value, cancellationToken);
            if (user is null) return Result.Failure<Guid>(UserErrors.UserNotFoundByEmail(request.UserEmail));

            var teamId = Guid.NewGuid();

            var teamResult = Team.Create(teamId, user, nameResult.Value, descriptionResult?.Value);
            var team = teamResult.Value;

            await _databaseContext.Set<Team>().AddAsync(team, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return teamId;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
