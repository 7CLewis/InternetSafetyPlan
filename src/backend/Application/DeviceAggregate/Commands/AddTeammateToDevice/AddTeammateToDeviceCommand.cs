using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public record AddTeammateToDeviceCommand(Guid DeviceId, Guid TeammateId) : ICommand<Unit>;
