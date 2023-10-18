using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.DomainEvents;

namespace Meeting.Application.Members.Events;

internal sealed class PerformBackgroundCheckWhenMemberRegisteredDomainEventHandler
    : IDomainEventHandler<MemberRegisteredDomainEvent>
{
    public Task Handle(MemberRegisteredDomainEvent notification, CancellationToken cancellationToken)
        => Task.CompletedTask;
}
