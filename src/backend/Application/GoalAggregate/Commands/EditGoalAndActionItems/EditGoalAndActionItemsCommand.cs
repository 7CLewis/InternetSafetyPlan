using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.GoalAggregate;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record EditGoalAndActionItemsCommand(Guid Id, Guid UltimateGoalId, string Name, GoalCategory Category, string? Description = null, DateTime? DueDate = null, List<EditGoalAndActionItemsCommand_ActionItem>? ActionItems = null) : ICommand<Unit>;
public record EditGoalAndActionItemsCommand_ActionItem(Guid Id, string Name, string? Description = null, DateTime? DueDate = null);

