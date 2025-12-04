using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.TeamAggregate.Queries;

public record GetTeammateByIdQuery(Guid TeamId, Guid TeammateId) : IQuery<TeammateByIdResponse>;
