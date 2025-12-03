using InternetSafetyPlan.Application.Base;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record AddActionItemToGoalCommand(Guid GoalId, string ActionItemName, string? ActionItemDescription = null, DateTime? ActionItemDueDate = null) : ICommand<Guid>;

