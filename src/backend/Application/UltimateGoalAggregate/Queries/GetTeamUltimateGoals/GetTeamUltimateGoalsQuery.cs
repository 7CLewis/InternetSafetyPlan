using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Queries;

public record GetTeamUltimateGoalsQuery(Guid TeamId, bool IncludeGoalsAndActions) : IQuery<List<TeamUltimateGoalsResponse>>;
