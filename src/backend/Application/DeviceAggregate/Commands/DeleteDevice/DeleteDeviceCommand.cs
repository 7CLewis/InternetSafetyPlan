using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public record DeleteDeviceCommand(Guid Id) : ICommand<Unit>;
