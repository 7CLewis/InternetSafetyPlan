using InternetSafetyPlan.Application.Base;
using MediatR;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Commands;

public record UpdateUltimateGoalInformationCommand(Guid UltimateGoalId, string Name, string? Description = null) : ICommand<Unit>;
