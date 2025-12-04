namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record ActionItemByIdResponse(Guid Id, Guid GoalId, string GoalName, string Name, DateTime DueDate, bool IsComplete, string? Description = null);
