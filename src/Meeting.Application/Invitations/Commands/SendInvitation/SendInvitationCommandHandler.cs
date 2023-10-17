using Meeting.Application.Abstractions;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Entities;
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

    public SendInvitationCommandHandler(IMemberRepository memberRepository, IMeetingRepository meetingRepository, IInvitationRepository invitationRepository, IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _memberRepository = memberRepository;
        _meetingRepository = meetingRepository;
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.MeetingId, cancellationToken);

        var meeting = await _meetingRepository.GetByIdWithCreatorAsync(request.MeetingId, cancellationToken);

        if (member is null || meeting is null)
        {
            //
        }

        Result<Invitation> invitationResult = meeting.SendInvitation(member);

        if (invitationResult.IsFailure)
        {
            //return;
        }

        _invitationRepository.Add(invitationResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _emailService.SendInvitationSentEmailAsync(
            member,
            meeting,
            cancellationToken);

        return Result.Success();
    }
}
