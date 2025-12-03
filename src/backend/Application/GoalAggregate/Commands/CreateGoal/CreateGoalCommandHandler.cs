using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;


public class CreateGoalCommandHandler : ICommandHandler<CreateGoalCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public CreateGoalCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = GoalName.Create(request.Name);
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

            Result<DueDate>? dueDateResult = null;
            if (request.DueDate != null)
            {
                dueDateResult = DueDate.Create((DateTime)request.DueDate);
                if (dueDateResult.IsFailure)
                {
                    // Log error
                    return Result.Failure<Guid>(dueDateResult.Error);
                }
            }

            var goalId = Guid.NewGuid();

            var goalResult = Goal.Create(goalId, request.UltimateGoalId, nameResult.Value, request.Category, descriptionResult?.Value, dueDateResult?.Value);
            if (goalResult.IsFailure)
            {
                // Log error
                return Result.Failure<Guid>(goalResult.Error);
            }

            var goal = goalResult.Value;

            await _databaseContext.Set<Goal>().AddAsync(goal, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return goalId;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
