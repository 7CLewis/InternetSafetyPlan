using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record DeleteGoalCommand(Guid Id) : ICommand<Unit>;
