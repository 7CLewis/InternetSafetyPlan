namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record TeamActionItemsResponse(Guid Id, Guid GoalId, string GoalName, string Name, DateTime DueDate, bool IsComplete, string? Description = null);
