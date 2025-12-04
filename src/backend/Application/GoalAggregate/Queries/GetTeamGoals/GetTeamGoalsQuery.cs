using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record GetTeamGoalsQuery(Guid TeamId) : IQuery<List<TeamGoalsResponse>>;
