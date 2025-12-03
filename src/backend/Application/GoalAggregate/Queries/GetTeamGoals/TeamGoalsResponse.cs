using InternetSafetyPlan.Domain.GoalAggregate;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record TeamGoalsResponse(Guid Id, Guid UltimateGoalId, string Name, GoalCategory Category, List<TeamGoalsResponse_ActionItem> ActionItems, bool IsComplete, string? Description = null, DateTime? DueDate = null);

public record TeamGoalsResponse_ActionItem(Guid Id, string Name, string Description, DateTime DueDate, bool IsComplete);
