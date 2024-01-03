using Meeting.Application.Abstractions.Messaging;
using Meeting.Application.Abstractions;
using Meeting.Domain.DomainEvents;
using Meeting.Domain.Entities;
using Meeting.Domain.Repositories;

namespace Meeting.Application.Meetings.Commands.CancelGathering;

internal sealed class OrderCancelledDomainEventHandler
    : IDomainEventHandler<OrderCancelledDomainEvent>
{
    private readonly IMeetingRepository _meetRepository;
    private readonly IEmailService _emailService;

    public OrderCancelledDomainEventHandler(IMeetingRepository meetingRepository, IEmailService emailService)
    {
        _meetRepository = meetingRepository;
        _emailService = emailService;
    }

    public async Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        var meeting = await _meetRepository.GetByIdAsync(
            notification.GatheringId,
            cancellationToken);

        if (meeting is null)
        {
            return;
        }

        foreach (Attendee attendee in meeting.Attendees)
        {
            await _emailService.SendGatheringCancelledEmailAsync(attendee, cancellationToken);
        }
    }
}

