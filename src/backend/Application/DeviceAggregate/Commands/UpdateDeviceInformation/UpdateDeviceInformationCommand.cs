using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using MediatR;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public record UpdateDeviceInformationCommand(Guid DeviceId, string Name, string Nickname, DeviceType DeviceType, List<Guid> TeammateIds, List<string> Tags) : ICommand<Unit>;
