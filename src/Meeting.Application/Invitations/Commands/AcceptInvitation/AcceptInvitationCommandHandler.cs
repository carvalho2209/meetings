using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Enums;
using Meeting.Domain.Errors;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Invitations.Commands.AcceptInvitation;

internal sealed class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand>
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

    public async Task<Result> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository
            .GetByIdWithCreatorAsync(request.MeetingId, cancellationToken);

        if (meeting is null)
        {
            return Result.Failure(
                DomainErrors.Meeting.NotFound(request.MeetingId));
        }

        var invitation = meeting.Invitations
            .FirstOrDefault(x => x.Id == request.MeetingId);

        if (invitation!.Status != InvitationStatus.Pending)
        {
            return Result.Failure(
                DomainErrors.Invitation.AlreadyAccepted(invitation.Id));
        }

        var attendeeResult = meeting.AcceptInvitation(invitation);

        if (attendeeResult.IsSuccess)
        {
            _attendeeRepository.Add(attendeeResult.Value);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
