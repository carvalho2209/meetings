using Meeting.Domain.Enums;

namespace Meeting.Application.Meetings.Commands.Queries.GetMeetingById;

public sealed record InvitationResponse(Guid InvitationId, InvitationStatus Status);