using Meeting.Domain.Primitives;

namespace Meeting.Domain.DomainEvents;
public sealed record MemberRegisteredDomainEvent(Guid MemberId) : IDomainEvent { }
