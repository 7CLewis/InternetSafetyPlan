namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Queries;

public record TeamUltimateGoalsResponse(Guid Id, Guid TeamId, string Name, string? Description, List<TeamUltimateGoalsResponse_Goal> Goals);
public record TeamUltimateGoalsResponse_Goal(Guid Id, string Name, string? Description, List<TeamUltimateGoalsResponse_Goal_Action> Actions);
public record TeamUltimateGoalsResponse_Goal_Action(Guid Id, string Name, bool isComplete, string? Description = null, DateTime? DueDate = null);
