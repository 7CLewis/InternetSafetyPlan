using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public record GetTeamByUserEmailQuery(string Email) : IQuery<TeamByUserEmailResponse?>;
