using MediatR;
using Meeting.Domain.Primitives;

namespace Meeting.Application.Abstractions.Messaging;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent;
