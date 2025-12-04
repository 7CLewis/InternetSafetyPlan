using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public class AddTagToDeviceCommandHandler : ICommandHandler<AddTagToDeviceCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public AddTagToDeviceCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(AddTagToDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tag =
                await _databaseContext
                .Set<Tag>()
                .SingleOrDefaultAsync(e => e.Name == request.TagName && e.Type == request.TagType, cancellationToken);
            if (tag is null) return Result.Failure<Unit>(SharedApplicationErrors.TagNotFound(request.TagName, request.TagType));

            var deviceWithTags = await _databaseContext
                .Set<Device>()
                .Include(e => e.Tags)
                .SingleOrDefaultAsync(e => e.Id == request.DeviceId, cancellationToken);
            if (deviceWithTags is null) return Result.Failure<Unit>(DeviceErrors.DeviceNotFound(request.DeviceId));

            var result = deviceWithTags.AddTag(tag);

            if (result.IsFailure) return Result.Failure<Unit>(result.Error);

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
