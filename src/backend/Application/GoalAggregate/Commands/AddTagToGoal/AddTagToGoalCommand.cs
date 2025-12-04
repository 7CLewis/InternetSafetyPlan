using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Shared;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record AddTagToGoalCommand(Guid GoalId, string TagName, TagType TagType) : ICommand<Unit>;

