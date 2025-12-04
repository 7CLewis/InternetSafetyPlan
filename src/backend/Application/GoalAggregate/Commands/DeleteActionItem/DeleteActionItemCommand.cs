using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record DeleteActionItemCommand(Guid GoalId, Guid ActionItemId) : ICommand<Unit>;

