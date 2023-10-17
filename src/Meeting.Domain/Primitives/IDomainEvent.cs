using MediatR;

namespace Meeting.Domain.Primitives;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}