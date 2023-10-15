using MediatR;
using Meeting.Domain.Shared;

namespace Meeting.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
