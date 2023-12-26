using Meeting.Application.Abstractions;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Invitations.Commands.SendInvitation;

internal sealed class SendInvitationCommandHandler : ICommandHandler<SendInvitationCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public SendInvitationCommandHandler(
        IMemberRepository memberRepository, 
        IMeetingRepository meetingRepository,
        IInvitationRepository invitationRepository, 
        IUnitOfWork unitOfWork, 
        IEmailService emailService)
    {
        _memberRepository = memberRepository;
        _meetingRepository = meetingRepository;
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        =>
            await Result.Combine(
                    Result.Create(
                        await _meetingRepository.GetByIdWithCreatorAsync(request.MeetingId, cancellationToken)),
                    Result.Create(
                        await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)))
                .Bind(t => t.Item1.SendInvitation(t.Item2))
                .Tap(_invitationRepository.Add)
                .Tap(() => _unitOfWork.SaveChangesAsync(cancellationToken))
                .Tap(invitation => _emailService.SendInvitationSentEmailAsync(
                    invitation.Member,
                    invitation.Meeting, cancellationToken));
}
