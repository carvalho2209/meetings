using Meeting.Application.Abstractions.Messaging;

namespace Meeting.Application.Meetings.Commands.Queries.GetMeetingById;

public sealed record GetMeetingByIdQuery(Guid MeetingId)  : IQuery<MeetingResponse>;