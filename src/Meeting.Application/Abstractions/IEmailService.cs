using Meeting.Domain.Entities;

namespace Meeting.Application.Abstractions;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(Member member, CancellationToken cancellationToken = default);

    Task SendInvitationSentEmailAsync(Member member, Domain.Entities.Meeting meeting, CancellationToken cancellationToken = default); 

    Task SendInvitationAcceptedEmailAsync(Domain.Entities.Meeting meeting, CancellationToken cancellationToken = default);

    Task SendGatheringCancelledEmailAsync(Attendee attendee, CancellationToken cancellationToken = default);
}