using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public class GetDeviceByIdQueryHandler : IQueryHandler<GetDeviceByIdQuery, DeviceByIdResponse>
{
    private readonly IDatabaseContext _databaseContext;
    public GetDeviceByIdQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<DeviceByIdResponse>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var devices = await _databaseContext
                .Set<Device>()
                .Include(device => device.Teammates)
                .Include(device => device.Tags)
                .SingleOrDefaultAsync(e => e.Id == request.DeviceId, cancellationToken);
            if (devices is null) return Result.Failure<DeviceByIdResponse>(DeviceErrors.DeviceNotFound(request.DeviceId));

            var teamDevicesResponse = devices.Adapt<DeviceByIdResponse>();

            return Result.Success(teamDevicesResponse);
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<DeviceByIdResponse>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
