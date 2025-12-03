using InternetSafetyPlan.Domain.GoalAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public class GetTeamGoalsQueryMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Goal, TeamGoalsResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description != null ? src.Description.Value : null)
            .Map(dest => dest.DueDate, src => src.DueDate != null ? src.DueDate.Value : (DateTime?)null);

        config.NewConfig<ActionItem, TeamGoalsResponse_ActionItem>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.DueDate, src => src.DueDate != null ? src.DueDate.Value : DateTime.MinValue);
    }
}
