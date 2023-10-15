using MediatR;

namespace Meeting.Application.Invitations.Commands.SendInvitation;

public sealed record SendInvitationCommand(Guid MemberId, Guid MeetingId) : IRequest;

