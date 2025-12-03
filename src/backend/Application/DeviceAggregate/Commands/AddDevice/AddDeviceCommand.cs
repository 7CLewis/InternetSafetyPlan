using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public record AddDeviceCommand(Guid TeamId, string Name, string Nickname, DeviceType DeviceType, List<Guid> TeammateIds, List<string> Tags) : ICommand<Guid>;
