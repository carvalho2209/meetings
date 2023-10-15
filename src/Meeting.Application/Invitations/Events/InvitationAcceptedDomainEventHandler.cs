using Meeting.Application.Abstractions;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.DomainEvents;
using Meeting.Domain.Repositories;

namespace Meeting.Application.Invitations.Events;

internal sealed class InvitationAcceptedDomainEventHandler
    : IDomainEventHandler<InvitationAcceptedDomainEvent>
{
    private readonly IEmailService _emailService;
    private readonly IMeetingRepository _meetingRepository;

    public InvitationAcceptedDomainEventHandler(IEmailService emailService, IMeetingRepository meetingRepository)
    {
        _emailService = emailService;
        _meetingRepository = meetingRepository;
    }

    public async Task Handle(InvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetByIdWithCreatorAsync(
            notification.MeetingId, cancellationToken);

        if (meeting is null)
        { 
            return;
        }

        await _emailService.SendInvitationAcceptedEmailAsync(meeting, cancellationToken);
    }
}
