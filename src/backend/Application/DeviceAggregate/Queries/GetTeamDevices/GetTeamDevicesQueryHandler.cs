using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.DeviceAggregate.Queries;

public class GetTeamDevicesQueryHandler : IQueryHandler<GetTeamDevicesQuery, List<TeamDevicesResponse>>
{
    private readonly IDatabaseContext _databaseContext;
    public GetTeamDevicesQueryHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<TeamDevicesResponse>>> Handle(GetTeamDevicesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var devices = await _databaseContext
                .Set<Device>()
                .Include(device => device.Teammates)
                .Include(device => device.Tags)
                .Where(e => e.TeamId == request.TeamId)
                .ToListAsync(cancellationToken);
            if (devices is null) return Result.Success(new List<TeamDevicesResponse>());

            var teamDevicesResponse = devices.Adapt<List<TeamDevicesResponse>>();

            return Result.Success(teamDevicesResponse);
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<List<TeamDevicesResponse>>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
