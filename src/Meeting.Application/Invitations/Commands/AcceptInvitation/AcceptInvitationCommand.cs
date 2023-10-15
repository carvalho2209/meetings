using MediatR;

namespace Meeting.Application.Invitations.Commands.AcceptInvitation;

public sealed record AcceptInvitationCommand(Guid MeetingId, Guid InvitationId) : IRequest;
