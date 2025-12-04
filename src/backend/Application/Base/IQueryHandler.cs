using InternetSafetyPlan.Domain.Base;
using MediatR;

namespace InternetSafetyPlan.Application.Base;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
