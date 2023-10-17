namespace Meeting.Domain.DomainEvents;

public sealed record InvitationAcceptedDomainEvent(
        Guid Id,
        Guid InvitationId, 
        Guid MeetingId) 
    : DomainEvent(Id);