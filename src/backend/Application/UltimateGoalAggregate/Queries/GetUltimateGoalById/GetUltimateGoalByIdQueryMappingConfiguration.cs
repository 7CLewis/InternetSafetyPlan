using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.UltimateGoalAggregate.Queries;

public class GetUltimateGoalQueryByIdMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UltimateGoal, UltimateGoalByIdResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description != null ? src.Description.Value : null);
    }
}
