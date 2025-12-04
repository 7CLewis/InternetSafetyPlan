using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Shared;
using MediatR;

namespace InternetSafetyPlan.Application.GoalAggregate.Commands;

public record RemoveTagFromGoalCommand(Guid GoalId, string TagName, TagType TagType) : ICommand<Unit>;

