namespace Meeting.Domain.Primitives;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _events = new(); 

    protected AggregateRoot(Guid id)
        : base(id)
    {
    }

    protected AggregateRoot() { }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _events.ToList();

    public void ClearDomainEvents () => _events.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _events.Add(domainEvent);
}
