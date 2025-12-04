using InternetSafetyPlan.Application.TeamAggregate.Queries;
using InternetSafetyPlan.Domain.GoalAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.GoalAggregate.Queries;

public class GetGoalByIdQueryMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Goal, GoalByIdResponse>()
            .MapWith(src => new GoalByIdResponse(
                src.Id, src.UltimateGoalId, 
                src.Name.Value, 
                src.Category,
                src.ActionItems.Adapt<List<GoalByIdResponse_ActionItem>>(),
                src.Description != null ? src.Description.Value : null, 
                src.DueDate != null ? src.DueDate.Value : null, 
                src.IsComplete)
            );

        config.NewConfig<ActionItem, GoalByIdResponse_ActionItem>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Description, src => src.Description != null ? src.Description.Value : null)
            .Map(dest => dest.DueDate, src => src.DueDate != null ? src.DueDate.Value : DateTime.MinValue);
    }
}
