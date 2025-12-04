using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.UserAggregate.Queries;

public record GetUserByIdQuery(Guid UserId) : IQuery<UserByIdResponse>;
