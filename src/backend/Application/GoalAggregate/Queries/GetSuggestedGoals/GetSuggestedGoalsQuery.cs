using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public record GetSuggestedGoalsQuery() : IQuery<List<SuggestedGoalsResponse>>;
