using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.GoalAggregate;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record CreateGoalAndActionItemsCommand(Guid UltimateGoalId, string Name, GoalCategory Category, string? Description = null, DateTime? DueDate = null, List<CreateGoalAndActionItemsCommand_ActionItem>? ActionItems = null) : ICommand<Guid>;
public record CreateGoalAndActionItemsCommand_ActionItem(string Name, string? Description = null, DateTime? DueDate = null);

