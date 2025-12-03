using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;

public class UpdateUltimateGoalInformationCommandHandler : ICommandHandler<UpdateUltimateGoalInformationCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public UpdateUltimateGoalInformationCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(UpdateUltimateGoalInformationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = UltimateGoalName.Create(request.Name);
            if (nameResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(nameResult.Error);
            }

            var descriptionResult = request.Description == null ? null : Description.Create(request.Description);
            if (descriptionResult != null && descriptionResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(descriptionResult.Error);
            }

            var ultimateGoalWithGoals = await _databaseContext
                .Set<UltimateGoal>()
                .Include(e => e.Goals)
                .SingleOrDefaultAsync(e => e.Id == request.UltimateGoalId, cancellationToken);
            if (ultimateGoalWithGoals is null) return Result.Failure<Unit>(UltimateGoalErrors.UltimateGoalNotFound(request.UltimateGoalId));

            var result = ultimateGoalWithGoals.UpdateInformation(nameResult.Value, descriptionResult?.Value);

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
