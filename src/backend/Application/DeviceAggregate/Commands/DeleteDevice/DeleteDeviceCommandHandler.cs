using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Domain.Base;
using InternetSafetyPlan.Domain.DeviceAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.DeviceAggregate.Commands;

public class DeleteDeviceCommandHandler : ICommandHandler<DeleteDeviceCommand, Unit>
{
    private readonly IDatabaseContext _databaseContext;
    public DeleteDeviceCommandHandler(IDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<Unit>> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var Device =
                await _databaseContext
                .Set<Device>()
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (Device is null) return Result.Failure<Unit>(DeviceErrors.DeviceNotFound(request.Id));

            Device.Delete();

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
