using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.GoalAggregate;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record CreateGoalCommand(Guid UltimateGoalId, string Name, GoalCategory Category, string? Description = null, DateTime? DueDate = null) : ICommand<Guid>;

