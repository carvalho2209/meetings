using Meeting.Domain.Primitives;

namespace Meeting.Domain.DomainEvents;

public abstract record DomainEvent(Guid Id) : IDomainEvent;
