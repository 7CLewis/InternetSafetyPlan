using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public record GetUserByEmailQuery(string Email) : IQuery<UserByEmailResponse?>;
