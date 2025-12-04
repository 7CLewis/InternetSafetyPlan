using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.TeamAggregate.Queries;

public record GetTeamByIdQuery(Guid Id) : IQuery<TeamByIdResponse>;
