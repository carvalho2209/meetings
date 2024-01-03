using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Errors;
using Meeting.Domain.Repositories;
using Meeting.Domain.Shared;

namespace Meeting.Application.Meetings.Commands.Queries.GetMeetingById;

internal sealed class GetMeetingByIdQueryHandler : IQueryHandler<GetMeetingByIdQuery, MeetingResponse>
{
    private readonly IMeetingRepository _meetRepository;

    public GetMeetingByIdQueryHandler(IMeetingRepository meetRepository) => _meetRepository = meetRepository;

    public async Task<Result<MeetingResponse>> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
    {
        var meeting = await _meetRepository.GetByIdAsync(request.MeetingId, cancellationToken);

        if (meeting is null)
        {
            return Result.Failure<MeetingResponse>(DomainErrors.Meeting.NotFound(request.MeetingId));
        }

        var response = new MeetingResponse(
            meeting.Id,
            meeting.Name,
            meeting.Location,
            $"{meeting.Creator.FirstName}" +
            $"{meeting.Creator.LastName}",
            meeting
                .Attendees
                .Select(attendee => new AttendeeResponse(
                    attendee.MemberId,
                    attendee.CreatedOnUtc)).ToList(),
            meeting
                .Invitations
                .Select(invitation => new InvitationResponse(
                    invitation.Id,
                    invitation.Status))
                .ToList());
        
        return response;
    }
}
