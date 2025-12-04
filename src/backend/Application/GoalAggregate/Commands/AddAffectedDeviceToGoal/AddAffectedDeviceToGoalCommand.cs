using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record AddAffectedDeviceToGoalCommand(Guid GoalId, Guid DeviceId) : ICommand<Unit>;

