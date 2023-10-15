using Meeting.Domain.Primitives;

namespace Meeting.Domain.DomainEvents;

public sealed record InvitationAcceptedDomainEvent(Guid InvitationId, Guid MeetingId) : IDomainEvent { }