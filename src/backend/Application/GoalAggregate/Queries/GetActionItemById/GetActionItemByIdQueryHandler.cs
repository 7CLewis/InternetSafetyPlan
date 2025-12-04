using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public class GetActionItemByIdQueryHandler : IQueryHandler<GetActionItemByIdQuery, ActionItemByIdResponse>
{
    private readonly IDatabaseContext _databaseContext;
    public GetActionItemByIdQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<ActionItemByIdResponse>> Handle(GetActionItemByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Goal> goals = [];
            var goalLists = await _databaseContext
                .Set<UltimateGoal>()
                .Include(e => e.Goals)
                .ThenInclude(goal => goal.ActionItems)
                .Where(e => e.TeamId == request.TeamId)
                .Select(e => e.Goals.ToList()).ToListAsync(cancellationToken);
            goalLists.ForEach(l => goals.AddRange(l));

            if (goals is null || goals.Count == 0) return Result.Failure<ActionItemByIdResponse>(GoalErrors.GoalNotFound(request.GoalId));

            var goal = goals.SingleOrDefault(goal => goal.Id == request.GoalId);
            if (goal is null) return Result.Failure<ActionItemByIdResponse>(GoalErrors.GoalNotFound(request.GoalId));

            var actionItem = goal.ActionItems.FirstOrDefault(actionItem => actionItem.Id == request.ActionItemId);
            if (actionItem is null) return Result.Failure<ActionItemByIdResponse>(GoalErrors.ActionItemNotFound(request.ActionItemId));

            return new ActionItemByIdResponse(actionItem.Id, goal.Id, goal.Name.Value, actionItem.Name.Value, actionItem.DueDate?.Value ?? DateTime.MinValue, actionItem.IsComplete, actionItem.Description?.Value);
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<ActionItemByIdResponse>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
