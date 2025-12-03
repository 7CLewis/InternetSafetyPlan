using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record GetGoalByIdQuery(Guid GoalId) : IQuery<GoalByIdResponse>;
