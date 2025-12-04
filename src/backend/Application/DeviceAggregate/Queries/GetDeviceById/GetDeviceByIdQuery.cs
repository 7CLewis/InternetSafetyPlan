using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public record GetDeviceByIdQuery(Guid DeviceId) : IQuery<DeviceByIdResponse>;
