using InternetSafetyPlan.Domain.DeviceAggregate;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public record DeviceByIdResponse(Guid Id, Guid TeamId, string Name, string Nickname, DeviceType Type, List<Guid> TeammateIds, List<string> Tags);
