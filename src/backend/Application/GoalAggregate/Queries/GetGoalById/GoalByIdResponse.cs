using InternetSafetyPlan.Domain.GoalAggregate;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record GoalByIdResponse(Guid Id, Guid UltimateGoalId, string Name, GoalCategory Category, List<GoalByIdResponse_ActionItem> ActionItems, string? Description, DateTime? DueDate, bool IsComplete);
public record GoalByIdResponse_ActionItem(Guid Id, string Name, string Description, DateTime DueDate);
