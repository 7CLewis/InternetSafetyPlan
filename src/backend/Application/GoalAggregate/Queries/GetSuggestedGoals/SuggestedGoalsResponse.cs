using InternetSafetyPlan.Domain.GoalAggregate;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record SuggestedGoalsResponse(Guid Id, Guid UltimateGoalId, string Name, GoalCategory Category, List<SuggestedGoalsResponse_ActionItem> ActionItems, bool IsComplete, string? Description = null, DateTime? DueDate = null);

public record SuggestedGoalsResponse_ActionItem(Guid Id, string Name, string Description, DateTime DueDate, bool IsComplete);
