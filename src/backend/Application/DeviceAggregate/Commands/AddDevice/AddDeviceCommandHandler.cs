using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.TeamAggregate;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public class AddDeviceCommandHandler : ICommandHandler<AddDeviceCommand, Guid>
{
    private readonly IDatabaseContext _databaseContext;
    public AddDeviceCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Guid>> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = ManufacturerName.Create(request.Name);
            if (nameResult.IsFailure) return Result.Failure<Guid>(nameResult.Error);

            var nicknameResult = Nickname.Create(request.Nickname);
            if (nicknameResult.IsFailure) return Result.Failure<Guid>(nicknameResult.Error);

            var deviceId = Guid.NewGuid();

            var deviceResult = Device.Create(deviceId, request.TeamId, nameResult.Value, nicknameResult.Value, request.DeviceType);
            if (deviceResult.IsFailure) return Result.Failure<Guid>(deviceResult.Error);

            var device = deviceResult.Value;

            if (request.TeammateIds.Any())
            {
                var team = await _databaseContext
                    .Set<Team>()
                    .Include(team => team.Teammates)
                    .FirstOrDefaultAsync(team => team.Id == request.TeamId, cancellationToken);

                if (team is null || !team.Teammates.Any()) return Result.Failure<Guid>(SharedDomainErrors.Empty("No teammates found in Team #" + request.TeamId));

                foreach (var teammateId in request.TeammateIds)
                {
                    device.AddTeammate(team.Teammates.First(teammate => teammate.Id == teammateId));
                }
            }

            if (request.Tags.Any())
            {
                foreach (var currentTag in request.Tags)
                {
                    var existingTag = await _databaseContext
                        .Set<Tag>()
                        .FirstOrDefaultAsync(tag => tag.Name == currentTag, cancellationToken);
                    if (existingTag is not null) device.AddTag(existingTag);
                    else device.AddTag(Tag.Create(Guid.NewGuid(), currentTag, TagType.DeviceMetadata).Value);
                }
            }

            await _databaseContext.Set<Device>().AddAsync(device, cancellationToken);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return deviceId;
        }
        catch (Exception exception)
        {
            Console.Write(exception.ToString());
            return Result.Failure<Guid>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
