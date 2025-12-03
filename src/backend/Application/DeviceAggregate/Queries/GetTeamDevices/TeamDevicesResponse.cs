using InternetSafetyPlan.Domain.DeviceAggregate;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public record TeamDevicesResponse(Guid Id, string Name, string Nickname, DeviceType Type, List<Guid> TeammateIds, List<string> Tags);
