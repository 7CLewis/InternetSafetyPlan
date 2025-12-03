using InternetSafetyPlan.Domain.Base;
using MediatR;

namespace InternetSafetyPlan.Application.Base;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

