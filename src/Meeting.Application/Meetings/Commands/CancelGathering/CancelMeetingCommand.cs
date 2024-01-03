using Meeting.Application.Abstractions.Messaging;

namespace Meeting.Application.Meetings.Commands.CancelGathering;

public sealed record CancelMeetingCommand(Guid GatheringId) : ICommand; 
