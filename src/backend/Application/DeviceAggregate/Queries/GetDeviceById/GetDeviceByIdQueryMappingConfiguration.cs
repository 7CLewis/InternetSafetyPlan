using InternetSafetyPlan.Domain.DeviceAggregate;
using Mapster;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public class GetDeviceByIdQueryMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Device, DeviceByIdResponse>()
            .MapWith(src =>
                new DeviceByIdResponse(src.Id, src.TeamId, src.Name.Value, src.Nickname.Value, src.Type, src.Teammates.Select(teammate => teammate.Id).ToList(), src.Tags.Select(tag => tag.Name).ToList())
            );
    }
}
