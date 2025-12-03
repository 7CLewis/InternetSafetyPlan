using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Queries;

public record GetUltimateGoalByIdQuery(Guid Id) : IQuery<UltimateGoalByIdResponse>;
