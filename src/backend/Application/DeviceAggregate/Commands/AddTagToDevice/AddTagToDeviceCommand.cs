using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Shared;
using MediatR;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public record AddTagToDeviceCommand(Guid DeviceId, string TagName, TagType TagType) : ICommand<Unit>;
