using Meeting.Domain.Primitives;

namespace Meeting.Domain.Repositories;

public interface IMeetingRepository
{
    Task<Entities.Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Entities.Meeting?> GetByIdWithCreatorAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Entities.Meeting?> GetByIdWithInvitationsAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Entities.Meeting gathering);

    void Remove(Entities.Meeting gathering);
}
