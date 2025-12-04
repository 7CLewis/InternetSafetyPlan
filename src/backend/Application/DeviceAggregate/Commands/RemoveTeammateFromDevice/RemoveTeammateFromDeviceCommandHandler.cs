using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Application.TeamAggregate;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.TeamAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public class RemoveTeammateFromDeviceCommandHandler : ICommandHandler<RemoveTeammateFromDeviceCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public RemoveTeammateFromDeviceCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(RemoveTeammateFromDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var teammate = await
                _databaseContext
                .Set<Teammate>()
                .SingleOrDefaultAsync(e => e.Id == request.TeammateId, cancellationToken);
            if (teammate is null) return Result.Failure<Unit>(TeamErrors.TeammateNotFound(request.TeammateId));

            var deviceWithTeammates = await _databaseContext
                .Set<Device>()
                .Include(e => e.Teammates)
                .SingleOrDefaultAsync(e => e.Id == request.DeviceId, cancellationToken);
            if (deviceWithTeammates is null) return Result.Failure<Unit>(DeviceErrors.DeviceNotFound(request.DeviceId));

            var result = deviceWithTeammates.RemoveTeammate(teammate);

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
