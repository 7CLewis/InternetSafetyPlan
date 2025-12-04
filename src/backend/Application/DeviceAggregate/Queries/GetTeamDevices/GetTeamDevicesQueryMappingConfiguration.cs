using InternetSafetyPlan.Domain.DeviceAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public class GetTeamDevicesMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Device, TeamDevicesResponse>()
            .Map(dest => dest.Name, src => src.Name.Value)
            .Map(dest => dest.Nickname, src => src.Nickname.Value)
            .Map(dest => dest.TeammateIds, src => src.Teammates.Select(teammate => teammate.Id).ToList())
            .Map(dest => dest.Tags, src => src.Tags.Select(tag => tag.Name).ToList());
    }
}
