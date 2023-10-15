using MediatR;
using Meeting.Domain.Entities;
using Meeting.Domain.Enums;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Invitations.Commands.AcceptInvitation;

internal sealed class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptInvitationCommandHandler(IMeetingRepository meetingRepository, IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork)
    {
        _meetingRepository = meetingRepository;
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetByIdWithCreatorAsync(request.MeetingId, cancellationToken);

        if (meeting is null)
        {
            return;
        }

        var invitation = meeting.Invitations
            .FirstOrDefault(x => x.Id == request.MeetingId);

        if (invitation is null || invitation.Status != InvitationStatus.Pending)
        {
            return;
        }

        Result<Attendee> attendeeResult = meeting.AcceptInvitation(invitation);

        if (attendeeResult.IsSuccess)
        {
            _attendeeRepository.Add(attendeeResult.Value);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
