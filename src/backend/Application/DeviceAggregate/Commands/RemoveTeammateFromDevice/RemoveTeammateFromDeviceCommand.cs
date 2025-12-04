using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public record RemoveTeammateFromDeviceCommand(Guid DeviceId, Guid TeammateId) : ICommand<Unit>;
