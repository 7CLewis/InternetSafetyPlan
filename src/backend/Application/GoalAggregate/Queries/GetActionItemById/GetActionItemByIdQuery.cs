using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record GetActionItemByIdQuery(Guid TeamId, Guid GoalId, Guid ActionItemId) : IQuery<ActionItemByIdResponse>;
