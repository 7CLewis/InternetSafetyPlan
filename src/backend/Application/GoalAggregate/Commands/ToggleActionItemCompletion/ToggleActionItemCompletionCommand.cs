using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record ToggleActionItemCompletionCommand(Guid GoalId, Guid ActionItemId) : ICommand<Unit>;
