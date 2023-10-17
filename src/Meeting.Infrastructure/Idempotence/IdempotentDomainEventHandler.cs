using MediatR;
using Meeting.Application.Abstractions.Messaging;
using Meeting.Domain.Primitives;
using Meeting.Persistence;
using Meeting.Persistence.OutBox;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Infrastructure.Idempotence;

public sealed class IdempotentDomainEventHandler
    <TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    private readonly INotificationHandler<TDomainEvent> _decorated;
    private readonly ApplicationDbContext _dbContext;

    public IdempotentDomainEventHandler(INotificationHandler<TDomainEvent> decorated, ApplicationDbContext context)
    {
        _decorated = decorated;
        _dbContext = context;
    }

    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        var consumer = _decorated.GetType().Name;

        if (await _dbContext.Set<OutboxMessageConsumer>()
                .AnyAsync(
                    outboxMessageConsumer =>
                        outboxMessageConsumer.Id == notification.Id &&
                        outboxMessageConsumer.Name == consumer,
                    cancellationToken))
        {
            return;
        }

        await _decorated.Handle(notification, cancellationToken);

        _dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}