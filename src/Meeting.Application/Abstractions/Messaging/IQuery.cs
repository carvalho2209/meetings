using MediatR;
using Meeting.Domain.Shared;

namespace Meeting.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
