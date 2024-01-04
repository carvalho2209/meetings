using Meeting.Application.Abstractions;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Invitations.Commands.SendInvitation;

internal sealed class SendInvitationCommandHandler(
    IMemberRepository memberRepository,
    IMeetingRepository meetingRepository,
    IInvitationRepository invitationRepository,
    IUnitOfWork unitOfWork,
    IEmailService emailService)
    : ICommandHandler<SendInvitationCommand>
{
    public async Task<Result> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        =>
            await Result.Combine(
                    Result.Create(
                        await meetingRepository.GetByIdWithCreatorAsync(request.MeetingId, cancellationToken)),
                    Result.Create(
                        await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)))
                .Bind(t => t.Item1.SendInvitation(t.Item2))
                .Tap(invitationRepository.Add)
                .Tap(() => unitOfWork.SaveChangesAsync(cancellationToken))
                .Tap(invitation => emailService.SendInvitationSentEmailAsync(
                    invitation.Member,
                    invitation.Meeting, cancellationToken));
}