using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Enums;

namespace Meeting.Application.Meetings.Commands.CreateMeeting;

public sealed record CreateMeetingCommand(
    Guid MemberId,
    MeetingType Type,
    DateTime ScheduleAtUct,
    string Name,
    string? Location,
    int? MaximumNumberOfAttendees,
    int? InvitationsValidBeforeInHours) : ICommand<Guid>;
