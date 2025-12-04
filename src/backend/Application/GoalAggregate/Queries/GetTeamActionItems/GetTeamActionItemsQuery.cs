using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record GetTeamActionItemsQuery(Guid TeamId) : IQuery<List<TeamActionItemsResponse>>;
