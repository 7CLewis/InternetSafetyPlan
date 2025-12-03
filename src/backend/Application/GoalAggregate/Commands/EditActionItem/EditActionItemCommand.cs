using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record EditActionItemCommand(Guid GoalId, Guid ActionItemId, string Name, string? Description = null, DateTime? DueDate = null) : ICommand<Unit>;
