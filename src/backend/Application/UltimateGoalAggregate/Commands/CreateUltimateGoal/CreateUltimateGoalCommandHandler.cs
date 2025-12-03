using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;

public class CreateUltimateGoalCommandHandler : ICommandHandler<CreateUltimateGoalCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public CreateUltimateGoalCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(CreateUltimateGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = UltimateGoalName.Create(request.Name);
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

            var ultimateGoalId = Guid.NewGuid();

            var ultimateGoalResult = UltimateGoal.Create(ultimateGoalId, request.TeamId, nameResult.Value, descriptionResult?.Value);
            var ultimateGoal = ultimateGoalResult.Value;

            await _databaseContext.Set<UltimateGoal>().AddAsync(ultimateGoal, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return ultimateGoalId;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }

    // TODO: Not sure about this practice. My idea is kinda SRP/SoC.
    // If I have multiple commands or queries in a single try/catch, I might 
    private Team? GetTeam(Guid teamId)
    {
        try
        {
            var team = _databaseContext.Set<Team>().SingleOrDefault(team => team.Id == teamId);

            return team;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return null;
        }
    }
}
