using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Queries.GetTeamUltimateGoals;

public class GetTeamUltimateGoalsQueryMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UltimateGoal, TeamUltimateGoalsResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.TeamId, src => src.TeamId)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description != null ? src.Description.Value : null)
            .Map(dest => dest.Goals, src => src.Goals);

        config.NewConfig<Goal, TeamUltimateGoalsResponse_Goal>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description != null ? src.Description.Value : null)
            .Map(dest => dest.Actions, src => src.ActionItems);

        config.NewConfig<ActionItem, TeamUltimateGoalsResponse_Goal_Action>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description != null ? src.Description.Value : null)
            .Map(dest => dest.DueDate, src => src.DueDate != null ? src.DueDate.Value : (DateTime?)null);
    }
}
