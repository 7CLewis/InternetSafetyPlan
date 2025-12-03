using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public record GetTeamDevicesQuery(Guid TeamId) : IQuery<List<TeamDevicesResponse>>;
