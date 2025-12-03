using InternetSafetyPlan.Domain.Base;
using MediatR;

namespace InternetSafetyPlan.Application.Base;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
