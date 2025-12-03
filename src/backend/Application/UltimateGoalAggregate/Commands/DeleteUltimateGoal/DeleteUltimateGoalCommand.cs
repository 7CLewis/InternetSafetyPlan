using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;

public record DeleteUltimateGoalCommand(Guid Id) : ICommand<Unit>;
