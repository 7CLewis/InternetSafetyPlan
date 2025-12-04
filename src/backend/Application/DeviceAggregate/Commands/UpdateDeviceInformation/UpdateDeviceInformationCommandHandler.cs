using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public class UpdateDeviceInformationCommandHandler : ICommandHandler<UpdateDeviceInformationCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;

    public UpdateDeviceInformationCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(UpdateDeviceInformationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var nameResult = ManufacturerName.Create(request.Name);
            if (nameResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(nameResult.Error);
            }

            var nicknameResult = Nickname.Create(request.Nickname);
            if (nicknameResult.IsFailure)
            {
                // Log error
                return Result.Failure<Unit>(nicknameResult.Error);
            }

            var device = await _databaseContext
                .Set<Device>()
                .Include(device => device.Teammates)
                .Include(device => device.Tags)
                .SingleOrDefaultAsync(e => e.Id == request.DeviceId, cancellationToken);
            if (device is null) return Result.Failure<Unit>(DeviceErrors.DeviceNotFound(request.DeviceId));

            var team = await _databaseContext
                .Set<Team>()
                .Include(team => team.Teammates)
                .FirstOrDefaultAsync(team => team.Id == device.TeamId, cancellationToken);

            if (team is null) return Result.Failure<Unit>(SharedDomainErrors.Empty("Team not found with ID #" + device.TeamId));

            var result = device.UpdateInformation(nameResult.Value, nicknameResult.Value, request.DeviceType);

            if (result.IsFailure) return Result.Failure<Unit>(result.Error);

            // Teammates
            foreach (var currentTeammate in team.Teammates)
            {
                var deviceTeammate = device.Teammates.FirstOrDefault(teammate => teammate.Id == currentTeammate.Id);
                var requestContainsTeammate = request.TeammateIds.Contains(currentTeammate.Id);

                if (deviceTeammate is not null 
                    && !requestContainsTeammate)
                {
                    device.RemoveTeammate(currentTeammate);
                }
                else if (deviceTeammate is null
                    && requestContainsTeammate)
                {
                    device.AddTeammate(currentTeammate);
                }
            }

            // Tags
            var deviceTags = device.Tags.ToList();

            foreach (var currentRequestTag in request.Tags)
            {
                if (deviceTags.FirstOrDefault(tag => tag.Name == currentRequestTag) is null)
                {
                    var tagResult = Tag.Create(Guid.NewGuid(), currentRequestTag, TagType.DeviceMetadata);
                    if (tagResult.IsFailure) return Result.Failure<Unit>(tagResult.Error);

                    var tag = tagResult.Value;
                    device.AddTag(tag);
                    _databaseContext.Set<Tag>().Add(tag);
                }
            }

            foreach(var currentDeviceTag in deviceTags)
            {
                if (!request.Tags.Contains(currentDeviceTag.Name))
                {
                    device.RemoveTag(currentDeviceTag);
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
                    else
                    {

                        var tagResult = Tag.Create(Guid.NewGuid(), currentTag, TagType.DeviceMetadata);
                        if (tagResult.IsFailure) return Result.Failure<Unit>(tagResult.Error);

                        var tag = tagResult.Value;
                        device.AddTag(tag);
                    }
                }
            }

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return Result.Failure<Unit>(SharedApplicationErrors.Database(exception.ToString()));
        }
    }
}
